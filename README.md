# Commerce Pricing Service API

A RESTful API service created as part of Columbia Sportswear SWE internship that provides real-time pricing information for products across different store locations, enabling customers to make informed purchasing decisions.

## Overview

The Commerce Pricing Service addresses the challenge customers face in determining whether the displayed price tag reflects the lowest available price at their current store location. This service provides a reliable, user-friendly way to retrieve current pricing information for specific items at specific stores.

### Key Features

- Real-time price lookup by item and store location
- Support for multiple pricing factors (style, store, channel, segment, color)
- Transaction history tracking for price changes
- Scalable architecture using Azure Cosmos DB
- RESTful API with OpenAPI 3.0 specification

### Target Audience

This API is designed to support end-user consumer applications. The service provides base pricing information without applying personal discounts or loyalty program benefits, ensuring transparent and consistent pricing data across all queries.

## Project Structure

```
commerce-pricing-service/src
├── domain/              # Business logic and entities
├── infrastructure/      # Data access and repositories
├── service/            # API endpoints and application services
├── changefeed/         # Change feed processors
```

## Architecture

High Level Diagram:

<img width="566" height="316" alt="image" src="https://github.com/user-attachments/assets/2ac0807e-994f-4e1e-9e9c-1d24c6531e1f" />

Low Level Diagram:

<img width="567" height="314" alt="image" src="https://github.com/user-attachments/assets/80e611f0-fb2a-4d9a-b3c8-8d361fe4e004" />


### Domain Layer (`domain/`)

The core business logic layer containing:
- **Business Entities**: Core models representing pricing concepts
- **Domain Services**: Business rules and workflows for pricing calculations
- **Utilities**: Shared helper functions and logging infrastructure
- **Independence**: No external dependencies on databases or frameworks

### Infrastructure Layer (`infrastructure/`)

The data access and persistence layer implementing:
- **Repository Pattern**: Collection managers for data access
- **CRUD Operations**: Standard create, read, update, delete operations
- **Database Configuration**: Cosmos DB integration and configuration

### Service Layer (`service/`)

API endpoints and application services exposing pricing functionality via:
- **RESTful API**: HTTP endpoints following REST principles
- **OpenAPI 3.0 Specification**: Auto-generated API documentation
- **ASP.NET Core 8**: Modern, high-performance web framework

### Change Feed Processing (`changefeed/`)

Real-time change tracking functionality for monitoring pricing updates and maintaining transaction history.

## Technology Stack

- **Framework**: .NET 8
- **API Specification**: OpenAPI 3.0
- **Database**: Azure Cosmos DB
- **CI/CD**: Azure DevOps Pipelines
- **Testing**: xUnit / NUnit (unit tests)

## Database Structure

The service utilizes Azure Cosmos DB with the following databases:

### 1. `commercepricingmaster`
Master pricing data containing current prices for all items across stores.

### 2. `commercepricingtransactions`
Transaction history tracking all price changes over time.
- **Partition Key**: `payload/id` (allows multiple transaction objects per partition)
- **Purpose**: Store complete audit trail of all pricing modifications

### 3. `leases`
Manages change feed lease tracking for distributed processing.

### Indexing Strategy

The databases implement selective indexing to optimize query performance while minimizing storage costs. Excluded paths are configured to skip indexing where full-text search is not required.

## Infrastructure as Code

The service infrastructure is managed through configuration files:

### Cosmos DB Configuration

- **Pipeline Config**: Single configuration file for CI/CD pipeline integration
- **Database Configs**: Four separate configuration files for each database environment (similar to Blazor project structure)

### Resource Organization

All Azure resources are organized within a dedicated **Resource Group**, providing centralized management and monitoring of related resources for the application.

## CI/CD Pipeline

The project maintains separate CI/CD pipelines for:

1. **Service Deployment**: Automated build, test, and deployment of the API service
2. **Cosmos DB Provisioning**: Infrastructure as code deployment for database resources

Multiple CI configuration files support different deployment scenarios and environments.

## API Documentation

The API follows OpenAPI 3.0 specifications and is versioned as **Commerce Pricing API v1**.

### Example Endpoints

```
GET /api/v1/pricing/{itemId}/store/{storeId}
```

Returns current pricing information for a specific item at a given store location.

### Pricing Factors

Prices may vary based on the following factors:
- Style
- Store location
- Sales channel
- Market segment
- Color/variant

## Contributing

This project was developed as part of an internship program with Columbia Sportswear.
