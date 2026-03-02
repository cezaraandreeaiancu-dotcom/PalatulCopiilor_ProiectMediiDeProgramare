
# Palatul Copiilor — Platformă Web + Aplicație Mobilă (.NET)

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](#)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](#)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Razor%20Pages-512BD4?logo=dotnet&logoColor=white)](#)
[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-Mobile-512BD4?logo=dotnet&logoColor=white)](#)
[![EF Core](https://img.shields.io/badge/EF%20Core-ORM-512BD4?logo=dotnet&logoColor=white)](#)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver&logoColor=white)](#)


Soluție software integrată pentru gestionarea activităților „Palatul Copiilor”, compusă din:
- **Platformă Web** (ASP.NET Core **Razor Pages**) — administrare departamente, activități, profesori, rezervări, autentificare/roluri.
- **Aplicație Mobilă** (**.NET MAUI**) — autentificare și vizualizarea rezervărilor utilizatorului (prin API + JWT).

> Proiect realizat în colaborare de **Hara Andrada Iarina** și **Iancu Cezara Andreea**.

---

## ✨ Funcționalități

### Web (Razor Pages)
- CRUD pentru entități:
  - **Department**
  - **Activity**
  - **Teacher**
  - **Reservation**
  - (opțional) **Review**
- Autentificare și autorizare cu **ASP.NET Identity**
- Roluri (ex: `Admin`, `Participant`) + seed inițial
- API minimal pentru mobile:
  - `POST /api/auth/login` → generează **JWT**
  - `GET /api/reservations/my` → rezervările utilizatorului autentificat (JWT required)
- **Swagger UI** în Development pentru testarea rapidă a endpoint-urilor

### Mobile (.NET MAUI)
- Login (email + parolă) → primește token JWT
- Afișare „Rezervările mele” (apel către API cu Bearer token)
- Configurare automată BaseAddress:
  - Android Emulator: `https://10.0.2.2:7127`
  - Desktop/iOS/mac: `https://localhost:7127`

---

## 🧱 Arhitectură (high level)

```
[ MAUI App ] --HTTP/JWT--> [ ASP.NET Core Web + Minimal API ]
|
| EF Core
v
[ SQL Server LocalDB ]
|
[ ASP.NET Identity ]
```

---

## 🧰 Tech Stack

- **.NET 8**
- **ASP.NET Core Razor Pages**
- **Entity Framework Core**
- **SQL Server LocalDB**
- **ASP.NET Core Identity**
- **JWT Bearer Authentication**
- **Swagger / OpenAPI**
- **.NET MAUI** (Android/iOS/Windows/MacCatalyst)

---

## 🚀 Getting Started

### 1) Cerințe
- Visual Studio 2022 (recomandat) cu workload-uri:
  - **ASP.NET and web development**
  - **.NET Multi-platform App UI development (MAUI)**
- **.NET SDK 8**
- SQL Server LocalDB (vine de obicei cu Visual Studio)

### 2) Clone
```bash
git clone https://github.com/cezaraandreeaiancu-dotcom/PalatulCopiilor_ProiectMediiDeProgramare.git
cd PalatulCopiilor_ProiectMediiDeProgramare
```
## 4️⃣ Setează JWT

În `Palatul_Copiilor/appsettings.json`:

- `Jwt:Key` (trebuie schimbată cu o cheie reală, lungă)
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:ExpireMinutes`

⚠️ **Nu publica chei reale pe GitHub.**

## 5️⃣ Rulează Web

- Setează `Palatul_Copiilor` ca Startup Project
- Rulează aplicația

Swagger disponibil la:


https://localhost:7127/swagger


---

## 6️⃣ Rulează Mobile

- Setează proiectul MAUI ca Startup Project
- Pornește Android Emulator
- Asigură-te că Web API rulează înainte

---

## 🔐 Cont Default (Seed)

- Email: `admin@palat.local`
- Parolă: `Admin123!`

---
## 🔌 API

### 🔐 Login

`POST /api/auth/login`

Body:

```json
{
  "email": "admin@palat.local",
  "password": "Admin123!"
}
```
### 📄 My Reservations

`GET /api/reservations/my`

Header:
Authorization: Bearer <token>

---

## ✅ Reguli de integritate

- Un utilizator nu poate rezerva aceeași activitate de două ori
- Un review este asociat unei singure rezervări

---

## 🗺️ Roadmap

- [ ] Listare activități în aplicația mobilă
- [ ] Rezervare direct din mobile
- [ ] Implementare Refresh Token
- [ ] Deploy pe Azure

---

## 🤝 Contributing

1. Fork repository
2. Creează branch nou (`feature/nume`)
3. Commit descriptiv
4. Creează Pull Request

---

## 👥 Authors

- Hara Andrada Iarina
- Iancu Cezara Andreea
