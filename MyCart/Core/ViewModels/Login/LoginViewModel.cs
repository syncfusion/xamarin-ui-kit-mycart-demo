using MyCart.Core.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyCart.ViewModels.Login
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private string email;

        private bool isInvalidEmail;

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the email ID from user in the login page.
        /// </summary>
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                SetProperty(ref email, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the entered email is valid or invalid.
        /// </summary>
        public bool IsInvalidEmail
        {
            get
            {
                return this.isInvalidEmail;
            }

            set
            {
                SetProperty(ref isInvalidEmail, value);
            }
        }

        #endregion
    }
}