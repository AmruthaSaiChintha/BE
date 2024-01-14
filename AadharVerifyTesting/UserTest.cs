using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class UserTest
{
    [Test]
    public void DataClass_Validation_ShouldPassForValidData()
    {
        // Arrange
        var data = new Data
        {
            Id = 1,
            FirstName = "amrutha",
            LastName = "chintha",
            Age = 25,
            Address = "123 Main St",
            Phone = "123-456-7890",
            AadharNumber = "123456789012",
            Email = "ammu@example.com"
        };

        var validationContext = new ValidationContext(data, null, null);
        var validationResult = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(data, validationContext, validationResult, true);

        Assert.IsTrue(isValid, "Validation should pass for valid data");
    }
    [Test]
    public void DataClass_Validation_ShouldPassForValidData1()  
    {
        // Arrange
        var validData = new Data
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Age = 25,
            Address = "123 Main St",
            Phone = "123-456-7890",
            AadharNumber = "123456789012",
            Email = "john.doe@example.com"
        };

        // Act
        var validationContext = new ValidationContext(validData, null, null);
        var validationResult = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(validData, validationContext, validationResult, true);

        // Assert
        Assert.IsTrue(isValid, "Validation should pass for valid data");
    }

    [Test]
    public void DataClass_Validation_ShouldPassForMinimumAge()
    {
        // Arrange
        var validData = new Data
        {
            Id = 2,
            FirstName = "Alice",
            LastName = "Smith",
            Age = 18, // Minimum age
            Address = "456 Oak St",
            Phone = "987-654-3210",
            AadharNumber = "987654321012",
            Email = "alice.smith@example.com"
        };

        // Act
        var validationContext = new ValidationContext(validData, null, null);
        var validationResult = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(validData, validationContext, validationResult, true);

        // Assert
        Assert.IsTrue(isValid, "Validation should pass for minimum age");
    }

    [Test]
    public void DataClass_Validation_ShouldPassForEmptyAddress()
    {
        // Arrange
        var validData = new Data
        {
            Id = 3,
            FirstName = "Bob",
            LastName = "Johnson",
            Age = 30,
            Address = "", // Empty address
            Phone = "555-123-4567",
            AadharNumber = "555111222333",
            Email = "bob.johnson@example.com"
        };

        // Act
        var validationContext = new ValidationContext(validData, null, null);
        var validationResult = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(validData, validationContext, validationResult, true);

        // Assert
        Assert.IsTrue(isValid, "Validation should pass for empty address");
    }
    [Test]
    public void DataClass_Validation_ShouldPassForMaxLengthAddress()
    {
        // Arrange
        var validData = new Data
        {
            Id = 4,
            FirstName = "Jane",
            LastName = "Doe",
            Age = 28,
            Address = new string('A', 150), 
            AadharNumber = "987654321012",
            Email = "jane.doe@example.com"
        };

        // Act
        var validationContext = new ValidationContext(validData, null, null);
        var validationResult = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(validData, validationContext, validationResult, true);

        // Assert
        Assert.IsTrue(isValid, "Validation should pass for maximum length address");
    }

    [Test]
    public void DataClass_Validation_ShouldPassForNullAadharNumber()
    {
        // Arrange
        var validData = new Data
        {
            Id = 5,
            FirstName = "Eva",
            LastName = "Johnson",
            Age = 35,
            Address = "789 Pine St",
            Phone = "555-987-6543",
            AadharNumber = null, // Null Aadhar number
            Email = "eva.johnson@example.com"
        };

        // Act
        var validationContext = new ValidationContext(validData, null, null);
        var validationResult = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(validData, validationContext, validationResult, true);

        // Assert
        Assert.IsTrue(isValid, "Validation should pass for null Aadhar number");
    }
    

    [Test]
    public void Id_ShouldHaveValidRange()
    {
        // Arrange
        var data = new Data();

        // Act
        data.Id = 1;

        // Assert
        Assert.That(data.Id, Is.GreaterThan(0));
    }


}
