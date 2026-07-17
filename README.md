# ERBedSystem

## Emergency Department Bed Management System

急診床位管理系統

## 專案介紹

ERBedSystem 是一套使用 ASP.NET Core Web API 開發的急診床位管理系統。

本專案透過軟體模擬急診臨床流程，協助管理：

- 病人候床狀態
- 床位使用狀態
- 自動派床流程
- 出院與床位清消流程

## 開發背景

開發者過去具有急診護理臨床經驗，實際參與急診病人照護與床位調度流程。

在臨床環境中，床位管理、病人流動以及資訊系統操作會直接影響醫療人員工作效率。

因此希望透過軟體開發能力，將臨床需求轉換為系統功能，設計更符合醫療流程的管理系統。

## 技術使用

### Backend

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- RESTful API

## Architecture

本專案採用分層架構：

```text
Client
   │
   ▼
Controller
   │
   ▼
Service
   │
   ▼
Repository
   │
   ▼
Entity Framework Core
   │
   ▼
SQLite Database
```

## 系統功能

### 🛏️ 病床管理

- 查詢所有病床狀態
- 新增病床
- 即時顯示 Available / Occupied / Cleaning 狀態
- 統計目前空床數量

---

### 👤 病人管理

- 新增病人
- 查詢病人資訊
- 管理 Waiting、Bedded、Discharged 狀態

---

### 🚑 自動派床

系統依據以下條件自動分配病床：

- 檢傷級數（Triage Level）
- 病人年齡
- ICU / Ward / Peds 床位

派床規則：

- 檢傷 1~2 級優先 ICU
- 成人病人安排 Ward
- 18 歲以下優先兒科床
- 若兒科床滿床，自動改派一般留觀床

---

### 🏥 出院流程

- 病人辦理出院
- 自動更新 Encounter
- 病床切換為 Cleaning 狀態
- 清消完成恢復 Available

---

### 🔄 轉床功能

- 驗證目標床位是否可使用
- 更新舊床、新床狀態
- 建立 Audit Log


## Design Concept

本專案並非單純 CRUD 練習，而是根據急診臨床流程設計。

本專案由具有急診護理師臨床經驗的開發者設計，將真實急診作業流程轉換為系統邏輯，包括：

- 檢傷級數優先派床
- ICU / Ward / Peds 分流
- 出院後床位清消流程
- 誤出院撤銷
- 病人轉床
- Audit Log 紀錄重要操作

希望透過系統設計，降低人工判斷成本，提升急診病床調度效率。
此專案目前仍持續開發中，未來將逐步加入更貼近真實醫療資訊系統的功能，例如：
## Future Improvements

目前已完成病床管理核心流程，未來預計持續擴充：

- [ ] JWT Login / Role-Based Authorization
- [ ] 即時床位 Dashboard
- [ ] SignalR 即時更新床位狀態
- [ ] Redis 快取熱門查詢
- [ ] Docker 部署
- [ ] Unit Test
- [ ] Integration Test
- [ ] Azure Cloud Deployment
- [ ] FHIR API 整合
> 本專案持續開發中，功能將依照實際醫療流程逐步擴充。

## Author

**黃千瑜**

Former Emergency Department Nurse → ASP.NET Core Backend Developer

- 2.5 years Emergency Department Nurse
- ASP.NET Core / C#
- Healthcare Information Systems
- RESTful API Development
