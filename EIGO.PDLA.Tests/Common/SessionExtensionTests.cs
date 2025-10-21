using EIGO.PDLA.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EIGO.PDLA.Tests.Common;

public class SessionExtensionsTests
{
    [Test]
    public void SessionExtensionsTest()
    {
        Mock<ISession> sessionMock = new();
        object toSerialize = new KeyValuePair<string, string>("key", "value");
        byte[] byteArray = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(toSerialize));

        sessionMock.Setup(_ => _.Set("test", It.IsAny<byte[]>()))
            .Callback<string, byte[]>((k, v) => byteArray = v);

        sessionMock.Setup(_ => _.TryGetValue("test", out byteArray))
            .Returns(true);

        sessionMock.Object.SetAsByteArray("test", toSerialize);
        Assert.That(sessionMock.Object, Is.Not.Null);
        var value = sessionMock.Object.GetAsByteArray<KeyValuePair<string, string>>("test");
        Assert.That(value, Is.Not.Null);
        Assert.That(byteArray, Is.Not.Null);
        Assert.AreEqual(value, toSerialize);

    }
}
