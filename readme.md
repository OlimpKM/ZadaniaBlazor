## Wesja na Microsoft Azure
![Aplikacja na Microsoft Azure](https://zadaniablazor-gydtckb4ffg6gmeh.canadacentral-01.azurewebsites.net)
* Uwaga: Pierwsze uruchomienie może zająć od 10 do 90 sekund (wynika to z ograniczeń darmowego planu F1 Free / tzw. "cold start").
# System zadań & Weather Dashboard

![.NET](https://img.shields.io/badge/.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Blazor](https://img.shields.io/badge/Blazor%20Web%20App-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

Aplikacja webowa, zbudowana w oparciu o architekturę **Blazor Web App (Interactive Server)**. Projekt łączy w sobie moduł do zarządzania zadaniami, dashboard pogodowy oraz zintegrowany system tożsamości.

[Aplikacja na Microsoft Azure](https://zadaniablazor-gydtckb4ffg6gmeh.canadacentral-01.azurewebsites.net/)
> Uwaga: Pierwsze uruchomienie może zająć od 10 do 90 sekund (wynika to z ograniczeń darmowego planu F1 Free / tzw. "cold start").

## 🚀 Kluczowe Funkcjonalności

### 1. System Zadań
Zaawansowany moduł typu LOB (Line of Business) umożliwiający pełne zarządzanie cyklem życia zadań:
* **CRUD**: Pełna obsługa tworzenia, edycji i usuwania zadań.
* **Monitorowanie Statusu**: Etykiety statusu (np. "Nowy, W realizacji, Zatrzymany, Zakończony") zintegrowane z logiką zadań.
* **Wyszukiwanie i Filtrowanie**: Intuicyjny interfejs z filtrowaniem dynamicznym po tytule oraz stanie realizacji.

### 2. Dashboard Pogodowy
Interaktywny moduł prezentujący dane meteorologiczne w czasie rzeczywistym:
* **Multi-city Support**: Możliwość zarządzania listą miast (dodanie, usunięcie miejscowości).
* **Prognoza Godzinowa**: Szczegółowe zestawienie temperatury i zjawisk atmosferycznych na nadchodzące 3 dni.
* **Integracja z API**: Automatyczne pobieranie danych za pośrednictwem dedykowanego serwisu `OpenWeatherMap`.

### 3. Personalizacja i Bezpieczeństwo
* **ASP.NET Core Identity**: Kompletny system logowania i rejestracji.
* **User Status Monitoring**: Wizualizacja stanu połączenia użytkownika (status dot) oraz integracja z `UserClaimsService`.
* **Security**: Wdrożona pełna ochrona **Antiforgery** oraz autoryzacja oparta na rolach i roszczeniach (Claims).

## 🛠️ Stos Technologiczny

* **Frontend**: Blazor Web App (C# / Razor Components)
* **Backend**: .NET 9 Core / REST API
* **Baza Danych**: MS SQL Server (Adventure Works Integration)
* **Zabezpieczenia**: Antiforgery Tokens, Bearer Authentication
* **Stylizacja**: CSS3 (custom layouts), Bootstrap Icons

## 👨‍💻 Podsumowanie

*Projekt zrealizowany w ramach optymalizacji narzędzi wewnętrznych i prezentacji nowoczesnych wzorców projektowych .NET.*# System Zadań & Weather Dashboard

