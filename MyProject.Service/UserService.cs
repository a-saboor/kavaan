using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using MyProject.Service.Helpers.Encryption;
using MyProject.Service.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IUserRoleRepository _userRoleRepository;
		private readonly IMail _email;
		private readonly IUnitOfWork _unitOfWork;

		public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMail email, IUnitOfWork unitOfWork)
		{
			this._userRepository = userRepository;
			this._userRoleRepository = userRoleRepository;
			this._email = email;
			this._unitOfWork = unitOfWork;
		}

		#region IUserService Members

		public IEnumerable<object> GetUserDropDown()
		{
			var user = _userRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from users in user
							   select new { value = users.ID, text = users.Name };
			return dropdownList;
		}
		public IEnumerable<User> GetUsers()
		{
            var users = _userRepository.GetAll().Where(i => i.IsDeleted == false);
			return users;
		}

		public int GetUsersForDashboard()
		{
			var users = _userRepository.GetAll().Where(i => i.IsDeleted == false).ToList();
			return users.Count();
		}

		public IEnumerable<User> GetRoleUsers(string name, string userName = null)
		{
			var userRole = _userRoleRepository.GetUserRoleByName(name);
			return userRole.Users.Where(g => g.Name.ToLower().Contains(userName.ToLower().Trim()));
		}

		public User GetUser(long id)
		{
			var user = _userRepository.GetById(id);
			return user;
		}

		public User GetUserByEmail(string email)
		{
			var user = _userRepository.GetUserByEmail(email);
			return user;
		}

		public User IsUserExist(string email, long id = 0)
		{
			var user = _userRepository.GetUserByEmail(email);
			return user;
		}

		public User GetByAuthCode(string authCode)
		{
			var user = _userRepository.GetByAuthCode(authCode);
			return user;
		}
		public bool EmailVerification(ref string Email, long ID, ref string message)
		{
			try
			{
				if (_userRepository.GetUserByEmail(Email, ID) == null)
				{

					message = "Email is unique";
					return true;
				}
				else
				{
					message = "Email address already exists! Please write another email address.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}
		public bool ContactVerification(ref string Code, ref string Contact, long ID, ref string message)
		{
			try
			{
				if (_userRepository.GetByContact(Code, Contact, ID) == null)
				{

					message = "Contact is unique";
					return true;
				}
				else
				{
					message = "Contact already exists! Please write another contact.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}
		public bool CreateUser(User user, ref string message, bool sendMail = false)
		{
			try
			{
				if (_userRepository.GetUserByEmail(user.EmailAddress) == null)
				{
					var password = user.Password;

					user.IsActive = true;
					user.IsDeleted = false;
					user.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_userRepository.Add(user);
					if (SaveUser())
					{
						user.Salt = (Int16)(user.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
						string HashPass = new Helpers.Encryption.Crypto().RetrieveHash(user.Password, Convert.ToString(user.Salt));
						user.Password = HashPass;
						_userRepository.Update(user);
						if (SaveUser())
						{
							if (sendMail)
							{
								_email.SendAccountCreationMail(user.EmailAddress, password, user.Name, "/Admin/Account/Login");
							}
							message = "User added successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later...";
							return false;
						}
					}
					else
					{
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
				}
				else
				{
					message = "User already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateUser(ref User user, ref string message)
		{
			try
			{
				if (_userRepository.GetUserByEmail(user.EmailAddress, user.ID) == null)
				{
					User CurrentUser = _userRepository.GetById(user.ID);

					CurrentUser.Name = user.Name;
					CurrentUser.EmailAddress = user.EmailAddress;
					CurrentUser.PhoneCode = user.PhoneCode;
					CurrentUser.MobileNo = user.MobileNo;
					CurrentUser.UserRoleID = user.UserRoleID;

					if (CurrentUser.Password != user.Password)
					{
						user.Salt = (Int16)(user.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
						string HashPass = new Crypto().RetrieveHash(user.Password, Convert.ToString(user.Salt));
						CurrentUser.Password = HashPass;
					}
					user = null;

					_userRepository.Update(CurrentUser);
					if (SaveUser())
					{
						user = CurrentUser;
						message = "User updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
				}
				else
				{
					message = "User already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteUser(long id, ref string message, bool softDelete = true)
		{
			try
			{
				User user = _userRepository.GetById(id);
				if (user.EmailAddress.Equals("admin@marudeals.com"))
				{
					message = "Admin User can't be deleted!";
					return false;
				}

				if (softDelete)
				{
					user.IsDeleted = true;
					_userRepository.Update(user);
				}
				else
				{
					_userRepository.Delete(user);
				}

				if (SaveUser())
				{
					message = "User deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public User Authenticate(string email, string password, ref string returnMessage)
		{
			var user = _userRepository.GetUserByEmail(email);
			if (user != null)
			{
				if (user.IsActive.HasValue && user.IsActive.Value)
				{
					string RetrievedPass = new Crypto().RetrieveHash(password, Convert.ToString(user.Salt.Value));
					if (RetrievedPass.Equals(user.Password))
					{
						returnMessage = "Login Successful";
						try
						{
							string updateMsg = string.Empty;
							user.AuthorizationCode = Guid.NewGuid() + "-" + Convert.ToString(user.ID);
							this.UpdateUser(ref user, ref updateMsg);
						}
						catch (Exception)
						{ }
						return user;
					}
					else
					{
						returnMessage = "Wrong Password!";
					}
				}
				else
				{
					returnMessage = "Account suspended!";
				}
			}
			else
			{
				returnMessage = "Invalid Email or Password!";
			}

			return null;
		}

		public bool ChangePassword(string oldPassword, string NewPassword, long userId, ref string message)
		{
			var user = _userRepository.GetById(userId);

			string HashOldPassword = new Crypto().RetrieveHash(oldPassword, Convert.ToString(user.Salt));

			if (user.Password.Equals(HashOldPassword))
			{
				string HashNewPassword = new Crypto().RetrieveHash(NewPassword, Convert.ToString(user.Salt));
				user.Password = HashNewPassword;
				_userRepository.Update(user);

				if (SaveUser())
				{
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
				}
			}
			else
			{
				message = "Entered old password is wrong !";
			}

			return false;
		}

		public bool ForgotPassword(string email, ref string message, string path)
		{
			try
			{
				var user = _userRepository.GetUserByEmail(email);

				if (user != null)
				{
					Crypto objCrypto = new Helpers.Encryption.Crypto();

					user.AuthorizationCode = objCrypto.Random(225);
					while (_userRepository.GetByAuthCode(user.AuthorizationCode) != null)
					{
						user.AuthorizationCode = objCrypto.Random(225);
					}
					user.AuthorizationExpiry = Helpers.TimeZone.GetLocalDateTime().AddMinutes(5);
					_userRepository.Update(user);
					if (SaveUser())
					{

						if (_email.SendForgotPasswordMail(user.ID, user.Name, user.EmailAddress, CustomURL.GetFormatedURL("Admin/Account/ResetPassword?auth=" + user.AuthorizationCode), path))
						{
							message = "Cool! Password recovery instruction has been sent to your email.";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later...";
						}
					}
				}
				else
				{
					message = "User email not exists!";
				}

				return false;
			}
			catch
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool ResetPassword(string password, long userId, ref string message)
		{
			var User = _userRepository.GetById(userId);

			string HashNewPassword = new Crypto().RetrieveHash(password, Convert.ToString(User.Salt));
			User.Password = HashNewPassword;
			User.AuthorizationCode = new Crypto().RetrieveHash(User.Name, Convert.ToString((Int16)(DateTime.UtcNow
				- new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds));
			_userRepository.Update(User);
			if (SaveUser())
			{
				message = "Account reset successfully";
				return true;
			}
			else
			{
				message = "Oops! Something went wrong. Please try later...";
			}

			return false;
		}

		public bool SaveUser()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion
	}

	public interface IUserService
	{
		IEnumerable<object> GetUserDropDown();
		IEnumerable<User> GetUsers();

		int GetUsersForDashboard();
		IEnumerable<User> GetRoleUsers(string name, string userName = null);
		User GetUser(long id);
		User GetUserByEmail(string email);
		User GetByAuthCode(string authCode);
		bool EmailVerification(ref string Email, long ID, ref string message);
		bool ContactVerification(ref string Code, ref string Contact, long ID, ref string message);
		bool CreateUser(User user, ref string message, bool sendMail = false);
		bool UpdateUser(ref User user, ref string message);
		bool DeleteUser(long id, ref string message, bool softDelete = true);
		User Authenticate(string email, string password, ref string returnMessage);
		bool ChangePassword(string oldPassword, string NewPassword, long userId, ref string message);
		bool ForgotPassword(string email, ref string message, string path);
		bool ResetPassword(string password, long userId, ref string message);
		bool SaveUser();
	}
}
