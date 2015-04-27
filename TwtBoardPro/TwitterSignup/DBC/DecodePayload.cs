/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */

namespace DeathByCaptcha
{
    /**
     * <seealso cref="T:DeathByCaptcha.Client.DecodeDelegate"/>
     * <summary>Helper class for asyncronous CAPTCHA decoding.</summary>
     */
    public class DecodePayload
    {
        /**
         * <value>Decoding callback.</value>
         */
        public DecodeDelegate Callback;

        /**
         * <value>Raw CAPTCHA image.</value>
         */
        public byte[] Image;

        /**
         * <value>Solving timeout (in seconds).</value>
         */
        public int Timeout;
    }
}
