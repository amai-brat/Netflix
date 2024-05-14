using AutoFixture.Kernel;

namespace Tests.Customizations;

public class DateOnlySpecimenBuilder: ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(DateOnly))
        {
            return new DateOnly(2021, 1, 1);
        }

        return new NoSpecimen();
    }
}