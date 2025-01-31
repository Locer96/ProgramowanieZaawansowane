# InventoryApp

## Authors
### Rafał Kuczmiński
### Dominik Latosiński

## Overview

Inventory App was created to solve the problem of having to keep track of various IT equipment in an accounting office, which is a branch of a worldwide company. The app is aimed at solving the issue of having to manually keep track of the location and assignment of IT equipment between employees and workstations through Excel files and replacing them with an all-in-one solution that is easier to manage and update.

Read further for a more in-depth overview of the software.

## Business Problem

In this specific branch, so far, there was no pre-determined way of keeping track of the hardware portfolio—that means which machine is assigned to which person and which desk has what equipment stored on it. This led to issues where machines could get lost (either due to negligence or malice). To solve this, initially, an Excel file was created, which kept track of the aforementioned issues. However, due to the nature of the office (open space and frequent switching of the machines in cases of issues), the process was very prone to errors and getting outdated quickly.
Example of Issues and Potential Errors

### A standard procedure in case of hardware or software issues is as follows:

- Employee A gets assigned a machine (1).
- Employee A encounters issues with his machine.
- A replacement machine (2) is issued while the IT team handles problems on machine (1).
- If the solution didn’t require the machine to be wiped of data, machine (1) is returned to the original user, and machine (2) is taken back to storage to be kept as spare.
- If the solution resulted in wiping data on machine (1), machine (2) is kept and replaces machine (1) as currently assigned.

The process is complicated due to the nature of replacing. Aimed mainly at the convenience of the user and trying to keep his work uninterrupted. If at any point, the IT admin makes an error with the current assignment, the following replacements cascade the problem into a bigger one until a big inventory check is made, confirming everyone's machines and what is being kept in stock.

## Solution

Switching to an SQL database with a user-friendly frontend greatly reduces the risk of errors and allows for easy and hassle-free updates of the current state of things (hardware assignment) on the fly with minimal effort. Switching from Excel to Inventory App reduces the risk of incorrect data by reducing user input on the whole process and replacing it with backend algorithms that update the database for them.
Key Benefits
- Efficiency: Reduces manual work and the need to double-check Excel for errors, duplicated data, or incorrect assignments.
- Visibility: Provides real-time insight into the current equipment assignment.
- Security & Accountability: Ensures equipment is properly managed and assigned.
- Scalability: Allows for dynamic changes in the number of user profiles within the system. Addition or removal is quick and easy to do.

## Business Impact

    Cost Saving: Less time is spent on manual tracking of equipment, which opens room for other tasks or handling tickets.
    Compliance: Is viewed positively in case of internal or external audits.
    Productivity: Keeping track of equipment allows for faster replacement in case of hardware/software issues.

## Expected Impact

Implementing Inventory App should significantly improve IT asset management efficiency within the company. By replacing manual Excel-based tracking with a centralized system, the app reduces administrative workload, minimizes errors, and enhances visibility into asset assignments. IT support teams will resolve issues faster, employees will receive equipment more efficiently, and overall accountability will increase. This leads to cost savings, improved compliance for audits, and a scalable solution that grows with the company’s needs.
Three Types of Users (Switch to Technical?)

## Inventory App uses three types of profiles to manage the equipment assignment.

### Regular

A regular user is an employee. This is the basic profile that most users have. It can review their assigned equipment.

### Support

A support profile is given to IT support employees. Their additional rights within the app allow them to edit the records of users to correct potential mistakes within the database.

### Administrator

Administrator accounts have the most rights within the system. Aside from the standard management of users, they can also edit the stock of available machines that can be assigned to users (the type of machines that support can assign to each person as their own).

## First Launch
### Local Development
1. Clone the repository to your local machine.
2. Ensure you have the necessary environment variables set up, including `AZURE_SQL_CONNECTIONSTRING`.
3. Run the application using your preferred IDE or `dotnet run` command.
4. On the first launch, the application will automatically apply any pending migrations to the database.
5. Multiple accounts with roles will be created on start of the application:
- Admin (Sufixes from 1 to 3)
   - **Email**: admin1@admin.com
   - **Password**: Admin@123
- Support (Sufixes from 1 to 3)
   - **Email**: support1@admin.com
   - **Password**: Support@123
- User (Sufixes from 1 to 25)
   - **Email**: user1@admin.com
   - **Password**: User@123
6. Use the admin account to log in and access administrator-only features.
7. Use the support account to log in and access support-only features.
8. Use the user account to log in and access authentication-only features.
9. Register other account to test registration

More information can be found in `DOCS.md`