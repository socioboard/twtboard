/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */

namespace DeathByCaptcha
{
    /**
     * <summary>Exception indicating that access to the API was denied due to invalid credentials, insufficied balance, or because the DBC account was banned.</summary>
     */
    public class AccessDeniedException : Exception
    {}
}
