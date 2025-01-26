### ******* Created By
#	Mohammad Al Chehab
#	Koussay Adawieye
#	Alaa Al Ghourabi
# ****************

# **Library Management System**

### **Description**  
This project is a microservices-based **Library Management System** that simplifies book borrowing, authentication, and service routing through a gateway. It ensures secure user authentication, efficient book inventory management, and seamless borrowing processes.

---

### **Project Architecture**  
The project consists of four main services:  
1. **Gateway Service**  
   - Acts as a central entry point for routing requests to the appropriate services.  
   - Uses **Ocelot** for API Gateway functionality and Swagger for route documentation.  

2. **Auth Service**  
   - Handles user authentication and authorization.  
   - Provides JWT-based secure access tokens.  

3. **Books Service**  
   - Manages the book inventory, including adding, updating, and retrieving book details.  

4. **Borrow Service**  
   - Manages book borrowing and return processes.  
   - Tracks user borrowing history and ensures policies are followed.

---

### **Features**  
- Secure **JWT Authentication** for user access.
- Gateway routing using **Ocelot** for efficient API management.
- Real-time inventory management with **Books Service**.
- Borrowing and return tracking with **Borrow Service**.
- Aggregated **Swagger UI** for all services.

---

### **Technologies Used**  
- **Gateway**: Ocelot, Swagger  
- **Auth Service**: ASP.NET Core Identity, JWT Authentication  
- **Books Service**: ASP.NET Core Web API, Entity Framework Core  
- **Borrow Service**: ASP.NET Core Web API, SQL Server  

---
