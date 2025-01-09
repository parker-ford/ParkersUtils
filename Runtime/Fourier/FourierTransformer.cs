
namespace ParkersUtils
{

    public enum FourierTransformerScaling
    {

    };

    public enum FourierTransformerShifting
    {

    };

    public enum FourierTransformerAlgorithm
    {

    };

    public struct FourierTransformerSettings
    {
        FourierTransformerScaling scaling;
        FourierTransformerShifting shifting;
        FourierTransformerAlgorithm algorithm;
    };

    public class FourierTransformer
    {
        public FourierTransformer()
        {

        }

        public FourierTransformer(FourierTransformerSettings settings)
        {

        }

        public void Forward()
        {

        }

        public void Inverse()
        {

        }
    }
}
