using AssetManager.Application.Features.Asset.Commands.CreateAsset;
using FluentValidation.TestHelper;
using Xunit;

namespace AssetManager.UnitTests.Features.Assets;

public class CreateAssetCommandValidatorTests
{
    private readonly CreateAssetCommandValidator _validator;

    public CreateAssetCommandValidatorTests()
    {
        _validator = new CreateAssetCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateAssetCommand("", "Desc", 100, "Category", "SN123");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Asset name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        // Arrange
        var command = new CreateAssetCommand("Laptop", "Desc", -50, "Category", "SN123");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
              .WithErrorMessage("Price cannot be negative.");
    }

    [Fact]
    public void Should_Have_Error_When_SerialNumber_Exceeds_MaxLength()
    {
        // Arrange - 101 karakterlik bir seri no
        var longSerial = new string('A', 101);
        var command = new CreateAssetCommand("Laptop", "Desc", 1000, "Category", longSerial);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SerialNumber);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateAssetCommand("MacBook Pro", "Excellent condition", 2500, "Electronics", "SN-999-XYZ");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}