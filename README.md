# MarketplaceServicesSuite
## Layered Architectures

The goal of this module is to provide an overview of the different layered architecture concepts from N-layer to Clean Architecture. Practical tasks include the implementation of the two services: Catalog and Carting with the testability and extensibility as a non-functional requirement (NFR).

### Task 1

Create BLL (business logic layer) and DAL (data-access layer) for **Carting Service**. Any implementation of the layered architecture can be used. Layers should be logically separated (via separate folders/namespaces).

**Functional Requirements:**

- Single entity – Cart
- Cart has a unique id which is maintained (generated) on the client-side.
- The following actions should be supported:
  - Get list of items of the cart object.
  - Add item to cart.
  - Remove item from the cart.
- Each item contains the following info:
  - Id – required, id of the item in external system (see Task 2), int.
  - Name – required, plain text.
  - Image – optional, URL and alt text.
  - Price – required, money.
  - Quantity – quantity of items in the cart.

**Non-functional Requirements (NFR):**

- Testability
- Extensibility

**Constraints:**

- NoSQL database for persistence layer (for example - https://www.litedb.org/).
- Layers should be logically separated

### Task 2

Create BLL (business logic layer) and DAL (data-access layer) for **Catalog Service**. You must follow Clean Architecture with physical layers separation (via separate DLLs).

**Constraints:**

- SQL database for persistence layer (for example - Microsoft SQL Server Database File).
- Layers should be physically separated.

**Non-functional Requirements (NFR):**

- Testability
- Extensibility

**Functional Requirements:**

- Key entities: Category, Item (aka Product).
- Category has:
  - Name – required, plain text, max length = 50.
  - Image – optional, URL.
  - Parent Category – optional
- The following operations are allowed for Category: get/list/add/update/delete.
- Item has:
  - Name – required, plain text, max length = 50.
  - Description – optional, can contain html.
  - Image – optional, URL.
  - Category – required, one item can belong to only one category.
  - Price – required, money.
  - Amount – required, positive int.
- The following operations are allowed for Item: get/list/add/update/delete.

## RESTful Web API

The goal of this module is to provide an overview of REST architecture style and its advanced topics like documentation and versioning. Practical task includes implementation of the APIs for both Catalog and Carting services.

### Task 1

Create REST based WEB API for **Catalog Service**.

**Functional Requirements:**

- The following actions should be supported:
  - List of categories
  - List of Items (filtration by category id and pagination)
  - Add category
  - Add item
  - Update category
  - Update item
  - Delete Item
  - Delete category (with the related items)

**Non-functional Requirements (NFR):**

- Testability
- All endpoints should correspond to at least the 2nd level of the maturity model. Few of the endpoints should follow the 3rd level.

### Task 2

Create REST based WEB API for **Carting Service**.

**Functional Requirements:**

- Versioning
  - Version 1 - API should support the following actions:
    - Get cart info.
      - Input params: cart unique Key (string).
      - Returns a cart model (cart key + list of cart items).
    - Add item to cart.
      - Input params: cart unique Key (string) + cart item model.
      - Returns 200 if item was added to the cart. If there was no cart for specified key – creates it. Otherwise returns a corresponding HTTP code.
    - Delete item from cart.
      - Input params: cart unique key (string) and item id (int).
      - Returns 200 if item was deleted, otherwise returns corresponding HTTP code.
  - Version 2 – the same as Version 1 but with the following changes:
    - Get cart info.
      - Returns a list of cart items instead of cart model.
- API documentation. Each API version should have its own documentation.

**Non-functional Requirements (NFR):**

- Testability
- Extensibility (via Versioning)
- Self-Documented API. API documentation should be generated during the build. XML-Docs could be used for providing details of the endpoints.

## Message Based Architecture. Message Broker

The goal of the course is to provide an overview of Message-Based Architecture and its key concept - Message Brokers (also called Message Oriented Middleware). Practical tasks include setting up one of the Message Broker tools and implementing Catalog and Carting services interaction using the selected message broker.

### Task 1

Choose and configure any message broker and setup it
- Azure Service Bus
- RabbitMQ
- Kafka
- Other

### Task 2

Create API for interaction with the chosen message broker.

**Functional Requirements:**

Implement the interaction between Catalog and Carting services via a message broker. When a user or external system changes the property’s value of any item in the catalog (e.g., name or price), it will be necessary to update this item in the basket.

**Recommendations**

- Choose any message broker
- Create a client with listener and publisher parts
- Call publisher from catalog service
- Call listener from carting service

**Non-functional Requirements (NRF):**

The new integration solution should guarantee message delivery between two services. In the case of failure, the solution should include correct work with the delayed messages.

## Security. Authentication & Authorization

Authentication and authorization are essential aspects of any modern application/ service or subsystem. In this module we will review different auth options and protocols as well as advanced concepts like SSO and MFA (Multi Factor Authentication). The practical part includes setup and configuration of your IAM (Identity and Access Management) server and role-based authentication implementation for your Catalog service.

You need to implement role-based security for your Catalog service endpoints using JWT tokens.

### Task 1

Setup identity management system. Identity Management System should have the following functionality:
- Predefined roles: Manager, Buyer 
- Predefined permissions:  
  - Buyer: Read 
  - Manager: Read, Create, Update, Delete 
- Generate Identity token 
- Check/verify Identity token 
- Implement Refresh token 

Note! You may use some cloud-based IMS (Identity Management Services) or look for existing free solutions like:
- https://github.com/DuendeSoftware/IdentityServer 
- https://github.com/IdentityServer/IdentityServer4  
- https://www.keycloak.org/ 
 
### Task 2

Catalog service - secure create/update/delete endpoints to be accessible for Manager role only. All Read (get) endpoints should not have any access limitations for the Manager role.

Carting service – all endpoints should be accessible for both roles but add a custom middleware needs to be added to log an identity access token detail.

Both services must be accessible via the same tokens.

## Containerization and Orchestration

The goal of this module is to make an overview of containerization and orchestration and best practices, create and run containers using Docker and provide overview about such concepts like orchestration and virtualization.

### Task 1

Based on previous modules, define all the external dependencies (as an example: message broker, SQL, or NoSQL database) and containerize them (use the pre-build container images). Catalog and Carting services should be reworked/reconfigured to work with the containerized versions of the dependencies.

### Task 2

Create a docker-compose file for all the services (Catalog and Carting) and external dependencies from Task1. Build, run, and test the compose file.
