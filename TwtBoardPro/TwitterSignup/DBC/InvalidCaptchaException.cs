/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */

namespace DeathByCaptcha
{
    /**
     * <summary>Exception indicating the CAPTCHA image was rejected due to being empty, or too big, or not a valid image at all.</summary>
     */
    public class InvalidCaptchaException : Exception
    {}
}
