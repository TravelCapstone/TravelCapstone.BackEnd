# Cóc Travel Capstone - CAPSTONE PROJECT FPT UNIVERSITY - FALL24
[![Build and deploy CI/CD container app to Azure Web App - coc-travel](https://github.com/duong-hong-quan/TravelCapstone.BackEnd/actions/workflows/dev_coc-travel.yml/badge.svg)](https://github.com/duong-hong-quan/TravelCapstone.BackEnd/actions/workflows/dev_coc-travel.yml)

## Introduction
The Travel Project is a tour management system that provides essential functionalities such as creating tours, checking in customers, processing booking payments, creating private tours, and combining tours. The project is developed using .NET Core API.

## Technologies Used
- .NET Core: The framework for building the API.
- Entity Framework: Object-Relational Mapping (ORM) tool for database management.
- Identity Framework: Manages users, passwords, profile data, roles, claims, tokens, email confirmation, and more.
- VNPAY && Momo: Integration for online payment processing.
- Hangfire: Background tasks.
- SignalR: ASP.NET SignalR is a library for ASP.NET developers that simplifies the process of adding real-time web functionality to applications.
- DinkToPDF: Create a report and export it to PDF.
- OfficeOpenXml: The library support for processing data from Excel file
- Google Authentication: Authenticate users by logging in with google. Use OAuth from Google.
- Firebase: The cloud storage
- Redis Cache: Redis is an open-source in-memory storage, used as a distributed, in-memory key–value database, cache and message broker, with optional durability

## Key Features
1. **Create Tour**: Allows users to create new travel tours.
2. **Customer Check-in**: Records customer attendance for tours.
3. **Booking Payment**: Enables customers to pay for their tour reservations.
4. **Create Private Tour**: Generates private tours for specific groups of customers.
5. **Tour Combination**: Combines multiple tours to create diverse tour packages.

## Usage
1. Clone project with this url [here](https://github.com/TravelCapstone/TravelCapstone.BackEnd.git)
2. Configure the database connection in the appsettings.json file.
3. Install dependencies: Ctrl + Shift + B.
4. Run migrations to set up the database: db migration with entity framework. (Nuget Console: update-database)
5. Refer to the documentation for detailed instructions on using specific features.

## Run App with Docker
1. docker build -t travelcapstone-api .
2. docker run -d -p 8080:80 --name travelcapstone-api-container travelcapstone-api
3. docker ps
Or using docker compose only with command: docker-compose up -d

### Contributing
**Back End**: **NET CORE API** V6.0
- [duong-hong-quan](https://github.com/duong-hong-quan) (Leader)
- [KhangPhamBM](https://github.com/KhangPhamBM)
- [ha-minh-quan](https://github.com/ha-minh-quan)
**Front End**: **ReactJS + ViteJS**
- [duong-hong-quan](https://github.com/duong-hong-quan)
- [bich-phuong](https://github.com/phuong1304)
## Contact
For any questions or contributions, please contact:
- Name: Hong Quan
- Email: hongquan.contact@gmail.com

#### Copyright &#169; 2024 - Dương Hồng Quân & Phạm Bùi Minh Khang & Hà Minh Quân
