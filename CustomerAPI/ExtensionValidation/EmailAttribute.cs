using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace CustomerAPI.ExtensionValidation
{
    public class EmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string email)
            {
                try
                {
                    var mailAddress = new MailAddress(email);
                    return mailAddress.Address == email;
                }
                catch (FormatException)
                {
                    throw new ValidationException("The provided email address is not valid.");

                }
            }

            throw new ValidationException("Invalid email value.");

        }
    }
}
