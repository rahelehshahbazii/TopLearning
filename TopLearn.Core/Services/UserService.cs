using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services
{
    public class UserService : IUserService
    {
        private TopLearnContext _Context;
        public UserService(TopLearnContext Context)
        {
            _Context = Context;
        }
        public bool ActiveAccount(string activecode)
        {
            var user = _Context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
            if (user == null || user.IsActive)
                return false;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _Context.SaveChanges();
            return true;
        }
        public int AddUser(User User)
        {
            _Context.Users.Add(User);
            _Context.SaveChanges();
            return User.UserId;
        }

        public int BalanceUserwallet(string userName)
        {
            int userId = GetUserIdByUserName(userName);

            //  ورود به کیف پول 
            var enter = _Context.Wallets.Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay).Select(w => w.Amount).ToList();
            var exit = _Context.Wallets.Where(w => w.UserId == userId && w.TypeId == 2).Select(w => w.Amount).ToList();
            return (enter.Sum() - exit.Sum());
        }
        public int GetUserIdByUserName(string userName)
        {
            return _Context.Users.Single(u => u.UserName == userName).UserId;
        }
        public void ChangeUserPassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }
        public bool CompareOldPassword(string oldPassword, string username)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _Context.Users.Any(u => u.UserName == username && u.Password == hashOldPassword);
        }
        public void EditProfile(string username, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagepath = "";
                if (profile.AvatarName != "DEfault.jpg")
                {

                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(imagepath))
                    {
                        File.Delete(imagepath);
                    }
                }
                profile.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }
            var user = GetUserByUserName(username);
            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.UserAvatar = profile.AvatarName;
            UpdateUser(user);
        }
        public EditProfileViewModel GetDataForEditProfileUser(string username)
        {
            return _Context.Users.Where(u => u.UserName == username).Select(u => new EditProfileViewModel()
            {
                AvatarName = u.UserAvatar,
                Email = u.Email,
                UserName = u.UserName
                //// RegisterDate=u.RegisterDate
            }).Single();
        }
        public SideBarUserPanelViewModel GetSideBarUserPanelData(string username)
        {
            return _Context.Users.Where(u => u.UserName == username).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate

            }).Single();
        }
        public User GetUserByActiveCode(string activeCode)
        {
            return _Context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public User GetUserByEmail(string email)
        {
            return _Context.Users.SingleOrDefault(u => u.Email == email);
        }
        public User GetUserByUserName(string username)
        {
            return _Context.Users.SingleOrDefault(u => u.UserName == username);
        }
       
        public bool IsExistEmail(string email)
        {

            return _Context.Users.Any(u => u.Email == email);
        }
        public bool IsExistUserName(string username)
        {
            return _Context.Users.Any(u => u.UserName == username);
        }
        public User LoginUser(LoginViewModel Login)
        {
            string HashPassword = PasswordHelper.EncodePasswordMd5(Login.Password);
            string email = FixedText.FixEmail(Login.Email);
            return _Context.Users.SingleOrDefault(u => u.Email == email && u.Password == HashPassword);
        }
        public void UpdateUser(User user)
        {
            _Context.Update(user);
            _Context.SaveChanges();
        }
        public List<WalletViewModel> GetWalletUser(string userName)
        {
            int userId = GetUserIdByUserName(userName);
            return _Context.Wallets.Where(w => w.IsPay && w.UserId == userId)
                .Select(w => new WalletViewModel
                {
                    Amount = w.Amount,
                    DateTime = w.CreateDate,
                    Description = w.Description,
                    Type = w.TypeId

                })
                .ToList();
        }
        //int IUserService.chargeWallet(string userName, int amount, string description, bool isPay)
        public int chargeWallet(string userName, int amount, string description, bool isPay)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = isPay,
                TypeId = 1,
                UserId = GetUserIdByUserName(userName),
            };
            return AddWallet(wallet);
        }

        // int IUserService.AddWallet(Wallet wallet)
        public int AddWallet(Wallet wallet)
        {
            _Context.Wallets.Add(wallet);
            _Context.SaveChanges();
            return wallet.WalletId;
        }
       public Wallet GetWalletByWalletId(int walletId)
        {
            return _Context.Wallets.Find(walletId);
        }
        public void UpdateWallet(Wallet wallet)
        {
            _Context.Wallets.Update(wallet);
            _Context.SaveChanges();
        }
        public UserForAdminViewModel GetUsers(int PageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _Context.Users;

            if (!String.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));

            }

            //Show Item In Page
            int take = 20;
            int skip = (PageId - 1) * take;

            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = PageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            return list;
        }
       public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User adduser = new User();
            adduser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            adduser.ActiveCode = NameGenerator.GenerateUniqCode();
            adduser.Email = user.Email;
            adduser.IsActive = true;
            adduser.RegisterDate = DateTime.Now;
            adduser.UserName = user.UserName;

            #region SaveAvatar
            if (user.UserAvatar != null)
            {
                string imagepath = "";
                adduser.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", adduser.UserAvatar);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }
            #endregion
            return AddUser(adduser);
        }
        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _Context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
               {
                    UserId=u.UserId,
                    AvatarName = u.UserAvatar,
                    Email = u.Email,
                    UserName = u.UserName,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()
               }).Single();
        }
       public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }
            if (editUser.UserAvatar != null)
            {
                //Delete Old Image 
                if (editUser.AvatarName != "Default.jpg")
                {

                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }

                }
                //Save New Image
                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }
            _Context.Users.Update(user);
            _Context.SaveChanges();
        }
        public User GetUserById(int userId)
        {
            return _Context.Users.Find(userId);
        }
        public UserForAdminViewModel GetDeleteUsers(int PageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _Context.Users.IgnoreQueryFilters().Where(u=>u.IsDelete);

            if (!String.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));

            }

            //Show Item In Page
            int take = 20;
            int skip = (PageId - 1) * take;

            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = PageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            return list;
        }
        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }
        public InformationUserViewModel GetUserInformation(string username)
        {
            var user = GetUserByUserName(username);
            InformationUserViewModel Information = new InformationUserViewModel();
            Information.UserName = username;
            Information.Email = user.Email;
            Information.RegisterDate = user.RegisterDate;
            Information.Wallet = BalanceUserwallet(username);
            return Information;
        }
        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel Information = new InformationUserViewModel();
            Information.UserName = user.UserName;
            Information.Email = user.Email;
            Information.RegisterDate = user.RegisterDate;
            Information.Wallet = BalanceUserwallet(user.UserName);
            return Information;
        }
    }
}