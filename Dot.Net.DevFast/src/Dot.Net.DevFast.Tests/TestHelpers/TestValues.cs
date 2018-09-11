namespace Dot.Net.DevFast.Tests.TestHelpers
{
    internal class TestValues
    {
        public const string BigString = @"
In this case, both linear polarizing filters P and A have their axes of transmission in the same direction. To obtain the 90 degree twisted nematic structure of the LC layer between the two glass plates without an applied electric field (OFF state), the inner surfaces of the glass plates are treated to align the bordering LC molecules at a right angle. This molecular structure is practically the same as in TN LCDs. However, the arrangement of the electrodes e1 and e2 is different. Because they are in the same plane and on a single glass plate, they generate an electric field essentially parallel to this plate. The diagram is not to scale: the LC layer is only a few micrometers thick and so is very small compared with the distance between the electrodes.
The LC molecules have a positive dielectric anisotropy and align themselves with their long axis parallel to an applied electrical field. In the OFF state (shown on the left), entering light L1 becomes linearly polarized by polarizer P. The twisted nematic LC layer rotates the polarization axis of the passing light by 90 degrees, so that ideally no light passes through polarizer A. In the ON state, a sufficient voltage is applied between electrodes and a corresponding electrical field E is generated that realigns the LC molecules as shown on the right of the diagram. Here, light L2 can pass through polarizer A.
In practice, other schemes of implementation exist with a different structure of the LC molecules - for example without any twist in the OFF state. As both electrodes are on the same substrate, they take more space than TN matrix electrodes. This also reduces contrast and brightness.[17]
Super-IPS was later introduced with better response times and colour reproduction.";

        public const string FixedCryptoKey = "vZxocNvGFa8Tig01mTAjdTYPH2lL/Qj+fiSyam5KQcw=";
        public const string FixedCryptoIv = "qtfGCd9nr2jCddpSlQLqgA==";
        public const string FixedCryptoPass = "p@ssw0rd";
        public const string FixedCryptoSalt = "s@ltString";
    }
}