# Playwright C# Automation Tests

This repository contains end-to-end UI tests for [AutomationInTesting.online](https://automationintesting.online) using **Playwright** and **NUnit** in **C#/.NET 8.0**.

The tests cover:

* Admin login functionality (valid/invalid credentials)
* Room reservation flow
* Verification of bookings in the admin report

---

## Technologies Used

* **.NET 8.0 / C#**
* **Playwright for .NET**
* **NUnit 3** (test framework)
* **Playwright Test Runner**
* **JSON** configuration and test data

---

## Project Structure

```
Playwrite_C#
│
├─ Pages/               # Page Object Models
│   ├─ BasePage.cs
│   ├─ HomePage.cs
│   ├─ AdminPage.cs
│   ├─ RoomPage.cs
│   └─ AdminReportPage.cs
│
├─ Tests/               # NUnit test classes
│   ├─ AdminLoginTests.cs
│   └─ ReservationTests.cs
│
├─ TestData/            # Test payloads
│   ├─ payload.json
│   └─ invalid_passwords.json
│
├─ config.json          # Base URL, headless setting, credentials
├─ PlaywrightSettings.cs # C# wrapper to load config.json
└─ PlaywrightTests.csproj
```

---

## Prerequisites

1. **.NET 8.0 SDK**
   [Download .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

2. **Install Playwright CLI**

```bash
dotnet tool install --global Microsoft.Playwright.CLI
```

3. **Install browsers**

```bash
playwright install
```

> This will download Chromium, Firefox, and WebKit for test automation.

---

## Installation

Clone the repository:

```bash
git clone <repository_url>
cd Playwrite_C#
```

Restore NuGet packages:

```bash
dotnet restore
```

---

## Configuration

* `config.json` – stores the base URL, headless option, and admin credentials:

```json
{
  "baseURL": "https://automationintesting.online",
  "headless": false,
  "auth": {
    "username": "admin",
    "password": "password"
  }
}
```

* `TestData/payload.json` – booking payload for Reservation tests.
* `TestData/invalid_passwords.json` – list of incorrect passwords for login tests.

> Paths in tests are resolved relative to the project root.

---

## Running Tests

1. Open terminal in project root:

```bash
cd I:\C#_Projects\Playwrite_C#
```

2. Clean & build the project (optional):

```bash
dotnet clean
dotnet build
```

3. Run all tests:

```bash
dotnet test
```

4. Run a specific test file (example: ReservationTests):

```bash
dotnet test --filter FullyQualifiedName~ReservationTests
```

---

## Notes / Known Behavior

* The first login uses credentials from `config.json`. Invalid login tests read passwords from `TestData/invalid_passwords.json`.
* After navigating back to the front page, tests re-login if needed before accessing admin reports.
* The test waits for network idle state to ensure pages are fully loaded before interacting.
* Selectors are hardcoded for **admin links**, **front page link**, and **room cards**.

---

## Test Flow Summary

1. **AdminLoginTests**

   * Check login with valid and invalid credentials.
   * Verify correct landing page (`/admin/rooms`) after successful login.

2. **ReservationTests**

   * Open front page, navigate to a room.
   * Generate random check-in/check-out dates.
   * Fill booking form with `payload.json`.
   * Verify booking appears in admin report.
   * Logout.
