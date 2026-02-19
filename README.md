# Playwright C# Automation Tests

This repository contains end-to-end UI tests for AutomationInTesting.online using Playwright and NUnit in C# with .NET 8.0.

The tests cover:

- Admin login functionality (valid and invalid credentials)
- Room reservation flow
- Verification of bookings in the admin report


## Technologies Used

- .NET 8.0 / C#
- Playwright for .NET
- NUnit 3 (test framework)
- Playwright Test Runner
- JSON configuration and test data


## Project Structure

Playwrite_C#
│
├─ Pages/               
│   ├─ BasePage.cs
│   ├─ HomePage.cs
│   ├─ AdminPage.cs
│   ├─ RoomPage.cs
│   └─ AdminReportPage.cs
│
├─ Tests/               
│   ├─ AdminLoginTests.cs
│   └─ ReservationTests.cs
│
├─ TestData/            
│   ├─ payload.json
│   └─ invalid_passwords.json
│
├─ config.json          
├─ PlaywrightSettings.cs
└─ PlaywrightTests.csproj

Pages contains Page Object Model classes.

Tests contains NUnit test classes.

TestData contains JSON payload files used for testing.

config.json stores base URL, headless option, and credentials.

PlaywrightSettings.cs loads configuration from config.json.


## Prerequisites

Install .NET 8.0 SDK  
https://dotnet.microsoft.com/download/dotnet/8.0

Install Playwright CLI:

dotnet tool install --global Microsoft.Playwright.CLI

Install Playwright browsers:

playwright install

This will download Chromium, Firefox, and WebKit.


## Installation

Clone the repository:

git clone <repository_url>
cd Playwrite_C#

Restore NuGet packages:

dotnet restore


## Configuration

config.json stores base URL, headless option, and admin credentials:

{
  "baseURL": "https://automationintesting.online",
  "headless": false,
  "auth": {
    "username": "admin",
    "password": "password"
  }
}

TestData/payload.json contains booking payload data for Reservation tests.

TestData/invalid_passwords.json contains a list of incorrect passwords for login tests.

Paths in tests are resolved relative to the project root.


## Running Tests

Open terminal in project root:

cd I:\C#_Projects\Playwrite_C#

Optional: clean and build the project:

dotnet clean
dotnet build

Run all tests:

dotnet test

Run a specific test class:

dotnet test --filter FullyQualifiedName~ReservationTests


## Notes / Known Behavior

The first login uses credentials from config.json.

Invalid login tests read passwords from TestData/invalid_passwords.json.

After navigating back to the front page, tests re-login if needed before accessing admin reports.

The tests wait for page load states before interacting with elements.

Selectors for admin links, front page navigation, and room cards are hardcoded.


## Test Flow Summary

AdminLoginTests

- Check login with valid credentials
- Check login with invalid credentials
- Verify correct landing page after successful login

ReservationTests

- Open front page
- Navigate to a room
- Generate random check-in and check-out dates
- Fill booking form using payload.json
- Verify booking appears in admin report
- Logout