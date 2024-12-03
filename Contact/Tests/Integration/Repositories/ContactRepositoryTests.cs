using Domain.Repositories.Relational;
using FluentAssertions;
using Infra.Persistence.Sql.Context;
using Infra.Persistence.Sql.Repositories;
using Integration.Context;
using Microsoft.Extensions.DependencyInjection;
using Shared.Builders;
using Xunit;

namespace Integration.Repositories;

public class ContactRepositoryTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly IContactRepository _contactRepository;

    public ContactRepositoryTests()
    {
        var serviceScopeFactory = DataContextTestFactory.CreateInMemoryServiceScopeFactory();
        _dataContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
        _contactRepository = new ContactRepository(serviceScopeFactory);
    }

    public void Dispose()
    {
        _dataContext.Dispose();
    }

    [Fact]
    public async Task ShouldSaveContactToDatabase()
    {
        // Arrange
        var contact = new ContactBuilder().Build();

        // Act
        var savedContactId = await _contactRepository.SaveAsync(contact);
        var savedContact = await _dataContext.Contacts.FindAsync(savedContactId);

        // Assert
        savedContactId.Should().NotBeNullOrWhiteSpace();
        savedContact.Should().NotBeNull();
        savedContact!.Name.Should().Be(contact.Name);
        savedContact.AreaCode.Should().Be(contact.AreaCode);
        savedContact.Phone.Should().Be(contact.Phone);
        savedContact.Email.Should().Be(contact.Email);
        savedContact.State.Should().Be(contact.State);
        savedContact.IsEnabled.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldUpdateContactInDatabase()
    {
        // Arrange
        var contact = new ContactBuilder().Build();
        await _dataContext.Contacts.AddAsync(contact);
        await _dataContext.SaveChangesAsync();

        // Act
        contact.Name = "Updated Contact";
        await _contactRepository.UpdateAsync(contact);
        var updatedContact = await _dataContext.Contacts.FindAsync(contact.Id);

        // Assert
        updatedContact.Should().NotBeNull();
        updatedContact!.Name.Should().Be(contact.Name);
    }

    [Fact]
    public async Task ShouldDeleteContactFromDatabase()
    {
        // Arrange
        var contact = new ContactBuilder().Build();
        await _dataContext.Contacts.AddAsync(contact);
        await _dataContext.SaveChangesAsync();

        // Act
        await _contactRepository.DeleteAsync(contact);
        var deletedContact = await _dataContext.Contacts.FindAsync(contact.Id);

        // Assert
        deletedContact.Should().NotBeNull();
        deletedContact!.IsEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldPermanentDeleteContactFromDatabase()
    {
        // Arrange
        var contact = new ContactBuilder().Build();
        await _dataContext.Contacts.AddAsync(contact);
        await _dataContext.SaveChangesAsync();

        // Act
        await _contactRepository.PermanentDelete(contact);
        var deletedContact = await _dataContext.Contacts.FindAsync(contact.Id);

        // Assert
        deletedContact.Should().BeNull();
    }

    [Fact]
    public async Task ShouldSearchContactsInDatabase()
    {
        // Arrange
        var contact1 = new ContactBuilder()
            .WithName("John Doe")
            .WithAreaCode(11)
            .WithIsEnabled(true)
            .Build();

        var contact2 = new ContactBuilder()
            .WithName("Jane Doe")
            .WithAreaCode(12)
            .WithIsEnabled(true)
            .Build();

        await _dataContext.Contacts.AddRangeAsync(contact1, contact2);
        await _dataContext.SaveChangesAsync();

        var filter = new ContactFilterBuilder()
            .WithAreaCode(11)
            .Build();

        // Act
        var result = await _contactRepository.SearchAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Total.Should().Be(1);
        result.Items.Should().ContainEquivalentOf(contact1);
        result.Items.Should().NotContainEquivalentOf(contact2);
    }

    [Fact]
    public async Task ShouldReturnNullWithNoError_WhenNoContactWasFounded()
    {
        var id = Guid.NewGuid().ToString();
        var result = await _contactRepository.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }
}
