using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string username);
        bool IsExistEmail (string email);
        int AddUser(User User);
        User LoginUser(LoginViewModel Login);
        User GetUserByEmail(string email);
        User GetUserById(int userId);
        User GetUserByActiveCode(string ActiveCode);
        void UpdateUser(User user);
        bool ActiveAccount(string activecode);
        int GetUserIdByUserName(string userName);
        User GetUserByUserName(string username);
        void DeleteUser(int userId);
        #region User Panel
        InformationUserViewModel GetUserInformation(string username);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string username);
        EditProfileViewModel GetDataForEditProfileUser(string username);
        void EditProfile(string username,EditProfileViewModel profile);
        bool CompareOldPassword(string oldPassword, string username);
        void ChangeUserPassword(string userName,string newPassword);
        #endregion

        #region Wallet
        int BalanceUserwallet(string userName);
        List<WalletViewModel> GetWalletUser(string userName);
        int chargeWallet(string userName , int amount, string description,bool isPay=false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);

        #endregion

        #region Admin Pannel
        UserForAdminViewModel GetUsers(int PageId = 1, string filterEmail = "", string filterUserName = "");

        UserForAdminViewModel GetDeleteUsers(int PageId = 1, string filterEmail = "", string filterUserName = "");
        
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);

        #endregion
    }
}
