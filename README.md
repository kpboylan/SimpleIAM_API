# SimpleIAM API

This is a minimal Identity and Access Management API for assigning users to clinical trial user groups. It is built with ASP.NET Core 8 and deployed to Azure via GitHub Actions.

---

## 🚀 Features

- User registration and authentication
- Assign users to predefined groups (e.g., Investigator, Site Admin)
- In-memory EF Core database (no persistence)
- Validates email, password strength, and group assignment
- xUnit test coverage integrated with GitHub Actions CI
- Deployed to Azure App Service

---

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- Optional: [Postman](https://www.postman.com/) or Swagger UI for API testing

---

## ⚙️ Running the API Locally

1. **Clone the repository:**

   git clone https://github.com/kpboylan/SimpleIAM_API
   cd simple-iam-api

2. **Run the API**
    dotnet run --project SimpleIAM_API

3. **Access Swagger UI**
    https://localhost:PORT/swagger

4. **Running Unit Tests**
    The API uses xUnit and Moq for testing.

5. **Deployment**
    Deployment is handled automatically using GitHub Actions.
    On every push to the master branch, the following happens:
    1. Project builds
    2. All unit tests run
    3. App is deployed to:
        https://simple-iam-api-staging-gpaxa6e9awddc0ac.westeurope-01.azurewebsites.net
    4. Use this URL to test with Swagger:
        https://simple-iam-api-staging-gpaxa6e9awddc0ac.westeurope-01.azurewebsites.net/swagger/index.html

## 📬 Example Endpoints

| Method | Route                       | Request Body                      | Description                        |
|--------|-----------------------------|-----------------------------------|------------------------------------|
| POST   | `/api/users/register`       | `{ "email": "", "password": "" }`| Register a new user                |
| POST   | `/api/users/authenticate`   | `{ "email": "", "password": "" }`| Authenticate user credentials      |
| POST   | `/api/users/assign-group`   | `{ "email": "", "groupId": 1 }`  | Assign a group to a user by ID     |
| GET    | `/api/users/{email}`        | _None_                            | Get a single user and their groups |
| GET    | `/api/users`                | _None_                            | Get all users and their groups     |

## 👥 Available User Groups

| ID | Group Name           |
|----|----------------------|
| 1  | Investigator         |
| 2  | Subinvestigator      |
| 3  | Biostatistician      |
| 4  | Quality Assurance    |
| 5  | Sponsor              |
| 6  | Safety Officer       |
| 7  | Site Admin           |
| 8  | Study Monitor        |

