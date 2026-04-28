using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Gear.Core.Application.Queries.GetBrands;
using Mihaylov.Api.Gear.Core.Application.Queries.GetCategories;
using Mihaylov.Api.Gear.Core.Application.Queries.GetCurrencies;
using Mihaylov.Api.Gear.Core.Application.Queries.GetGroups;
using Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;
using Mihaylov.Api.Gear.Core.Application.Queries.GetShops;
using Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;
using Mihaylov.Api.Gear.Core.Domain.Enums;
using DbC = Mihaylov.Api.Gear.Core.Domain.Lookups;
using DbCe = Mihaylov.Api.Gear.Core.Domain.Entities;

namespace Mihaylov.Api.Gear.Core.Application;

public static class _DbModelMapping
{
    public static IServiceCollection RegisterDbMapping(this IServiceCollection services)
    {
        TypeAdapterConfig<DbCe.Trip, Trip>.NewConfig()
            .Map(dest => dest.Id, src => src.TripId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.Notes, src => src.Notes)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        TypeAdapterConfig<Trip, DbCe.Trip>.NewConfig()
            .Map(src => src.TripId, dest => dest.Id)
            .Map(src => src.Title, dest => dest.Title)
            .Map(src => src.Year, dest => dest.Year)
            .Map(src => src.Notes, dest => dest.Notes)
            .Map(src => src.CreatedAt, dest => dest.CreatedAt);

        TypeAdapterConfig<Trip, TripFull>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Year, src => src.Year)
            .Map(dest => dest.Notes, src => src.Notes)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .TwoWays();

        TypeAdapterConfig<DbCe.GearNode, GearNode>.NewConfig()
            .Map(dest => dest.Id, src => src.GearNodeId)
            .Map(dest => dest.TripId, src => src.TripId)
            .Map(dest => dest.ParentId, src => src.ParentId)
            .Map(dest => dest.NodeType, src => (NodeType)src.NodeTypeId)
            .Map(dest => dest.GroupId, src => src.GroupId)
            .Map(dest => dest.GroupName, src => src.Group != null ? src.Group.Name : null)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : null)
            .Map(dest => dest.InventoryItemId, src => src.InventoryItemId)
            .Map(dest => dest.InventoryItem, src => src.InventoryItem != null ?
                string.Concat(src.InventoryItem.Name, src.InventoryItem.Brand != null ?
                    string.Concat(" (", src.InventoryItem.Brand.Name, ")") : null) : null)
            .Map(dest => dest.IsInventoryItemActive, src => src.InventoryItem != null ? (src.InventoryItem.ItemStatusId == (int)ItemStatus.Active) : (bool?)null)
            .Map(dest => dest.InventoryKitContents, src => src.InventoryItem != null ? src.InventoryItem.KitContents : null)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.IsPacked, src => src.IsPacked)
            .Map(dest => dest.IsExcluded, src => src.IsExcluded)
            .Map(dest => dest.IsRequired, src => src.IsRequired)
            .IgnoreNonMapped(true);

        TypeAdapterConfig<GearNode, DbCe.GearNode>.NewConfig()
            .Map(src => src.GearNodeId, dest => dest.Id)
            .Map(src => src.TripId, dest => dest.TripId)
            .Map(src => src.ParentId, dest => dest.ParentId)
            .Map(src => src.NodeTypeId, dest => (int)dest.NodeType)
            .Map(src => src.GroupId, dest => dest.GroupId)
            .Map(src => src.CategoryId, dest => dest.CategoryId)
            .Map(src => src.InventoryItemId, dest => dest.InventoryItemId)
            .Map(src => src.Quantity, dest => dest.Quantity)
            .Map(src => src.IsPacked, dest => dest.IsPacked)
            .Map(src => src.IsExcluded, dest => dest.IsExcluded)
            .Map(src => src.IsRequired, dest => dest.IsRequired)
            .IgnoreNonMapped(true);

        TypeAdapterConfig<DbCe.InventoryItem, Inventory>.NewConfig()
            .Map(dest => dest.Id, src => src.InventoryItemId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.BrandId, src => src.BrandId)
            .Map(dest => dest.Brand, src => src.Brand != null ? src.Brand.Name : null)
            .Map(dest => dest.ShopId, src => src.ShopId)
            .Map(dest => dest.Shop, src => src.Shop != null ? src.Shop.Name : null)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.Category, src => src.Category.Name)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.CurrencyId, src => src.CurrencyId)
            .Map(dest => dest.Currency, src => src.Currency != null ? src.Currency.Symbol : null)
            .Map(dest => dest.PurchaseDate, src => src.PurchaseDate)
            .Map(dest => dest.ItemStatus, src => (ItemStatus)src.ItemStatusId)
            .Map(dest => dest.KitContents, src => src.KitContents);

        TypeAdapterConfig<Inventory, DbCe.InventoryItem>.NewConfig()
             .Map(src => src.InventoryItemId, dest => dest.Id)
             .Map(src => src.Name, dest => dest.Name)
             .Map(src => src.Description, dest => dest.Description)
             .Map(src => src.BrandId, dest => dest.BrandId)
             .Map(src => src.ShopId, dest => dest.ShopId)
             .Map(src => src.CategoryId, dest => dest.CategoryId)
             .Ignore(src => src.Category)
             .Map(src => src.Price, dest => dest.Price)
             .Map(src => src.CurrencyId, dest => dest.CurrencyId)
             .Map(src => src.PurchaseDate, dest => dest.PurchaseDate)
             .Map(src => src.ItemStatusId, dest => (int)dest.ItemStatus)
             .Ignore(src => src.ItemStatus)
             .Map(src => src.KitContents, dest => dest.KitContents);

        TypeAdapterConfig<DbCe.KitContentItem, KitContentItem>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .TwoWays();

        TypeAdapterConfig<DbC.Category, Category>.NewConfig()
            .Map(dest => dest.Id, src => src.CategoryId)
            .Map(dest => dest.Name, src => src.Name)
            .TwoWays();

        TypeAdapterConfig<DbC.Currency, Currency>.NewConfig()
            .Map(dest => dest.Id, src => src.CurrencyId)
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Symbol, src => src.Symbol)
            .Map(dest => dest.IsDefault, src => src.IsDefault)
            .TwoWays();

        TypeAdapterConfig<DbC.Brand, Brand>.NewConfig()
            .Map(dest => dest.Id, src => src.BrandId)
            .Map(dest => dest.Name, src => src.Name)
            .TwoWays();

        TypeAdapterConfig<DbC.Group, Group>.NewConfig()
            .Map(dest => dest.Id, src => src.GroupId)
            .Map(dest => dest.Name, src => src.Name)
            .TwoWays();

        TypeAdapterConfig<DbC.Shop, Shop>.NewConfig()
            .Map(dest => dest.Id, src => src.ShopId)
            .Map(dest => dest.Name, src => src.Name)
            .TwoWays();

        return services;
    }
}
