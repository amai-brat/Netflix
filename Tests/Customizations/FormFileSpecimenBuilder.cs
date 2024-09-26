using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;

namespace Tests.Customizations;

public class FormFileSpecimenBuilder: ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(IFormFile))
        {
            var data = new byte[1000];
            Array.Fill(data, (byte)Random.Shared.Next(0,2));
            var stream = new MemoryStream();
            return new FormFile(stream,0,stream.Length,"test","test");
        }

        return new NoSpecimen();
    }
}