# Book Shop Management System

**The Book Shop Management System** is a comprehensive application developed using C# and the .NET Framework. This system is designed to manage book inventory, customer data, and sales transactions, providing both users and administrators with a seamless experience.

## Features
- **User Dashboard**: An intuitive interface for customers to view and manage their book purchases.
- **Admin Dashboard**: A dedicated interface for administrators to oversee system operations, manage books, and handle customer inquiries.
- **User Authentication**: Secure login, password reset, and registration functionalities to protect user information.
- **Book Management**: Detailed views of book information with capabilities for purchase and billing.
- **Real-Time Updates**: The system automatically updates with changes in the database, ensuring users have access to the latest information.
- **Crystal Reports Integration**: Utilizes Crystal Reports for generating insightful sales and inventory reports.

## Database Management
The application uses Microsoft SQL Server Management for database management, which includes:
- **Tables**:
  - **Customer**: Stores customer information.
  - **Book**: Contains details about available books.
  - **OrderDetails**: Tracks customer orders and transactions.
- **Triggers**: Implemented in the Customer and OrderDetails tables to auto-generate primary keys, ensuring data integrity.

## Technologies Used
- **C#**: Primary programming language for the application.
- **.NET Framework**: Framework used to build the application.
- **Microsoft SQL Server**: Database management system.
- **Crystal Reports**: For reporting functionalities.

