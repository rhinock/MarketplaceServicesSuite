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
