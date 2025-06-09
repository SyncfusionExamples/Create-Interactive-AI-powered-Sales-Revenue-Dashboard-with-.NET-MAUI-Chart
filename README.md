#  Create Interactive Sales Revenue Dashboard with .NET MAUI & Syncfusion Charts
 Discover how to build an interactive, AI-powered sales revenue and analytics dashboard using .NET MAUI and Syncfusion controls. This open-source project is a complete end-to-end guide to developing a cross-platform business dashboard for Windows, Android, iOS, and macOS with rich data visualizations and predictive insights.


##  Overview

In today’s competitive business environment, dashboards need to do more than just display numbers—they need to predict, explain, and enable decisions. This project shows you how to use the power of .NET MAUI and Syncfusion UI components to build a cross-platform dashboard that brings data to life.

The dashboard offers revenue trends, product sales breakdowns, regional analysis, and AI-driven sales forecasting, all wrapped in a beautiful, responsive UI. Whether you're a business analyst, developer, or enthusiast, this project will help you understand how to build scalable dashboards using modern .NET tools.


##  Key Features

###  Rich Visualizations
- Revenue over time with smooth area charts
- Quantity sold with dynamic column bars
- Top product breakdown with a doughnut chart
- Confidence level visualization using radial gauges
- Region-wise sales displayed on interactive maps

###  AI-Driven Forecasting
- Uses Azure OpenAI (GPT) to generate daily sales predictions
- Includes confidence intervals and anomaly flags
- Explains predictions in natural language using NLP
- Fallback logic for offline/mock prediction data

###  Smart Assistant
- Embedded AI assistant (chat UI) powered by Syncfusion `SfAIAssistView`
- Users can ask natural-language questions about sales data
- Suggestions for commonly asked queries

###  Data Grids & KPIs
- Interactive `SfDataGrid` for orders and product lists
- Live sorting, filtering, grouping, and paging
- Summary cards for key metrics (revenue, profit, margin, growth)

##  Technologies Used

- **.NET MAUI** – Cross-platform app framework
- **Syncfusion .NET MAUI Components** – Charts, Gauges, Maps, DataGrid, AI Assist
- **Azure OpenAI / ChatGPT API** – For NLP and forecasting
- **MVVM Pattern** – Clean architecture with reactive ViewModels
- **C# & XAML** – UI + logic

##  Syncfusion Controls Used

This project uses a rich set of Syncfusion .NET MAUI controls to deliver a highly visual and interactive dashboard experience:

- [**SfCartesianChart**](https://help.syncfusion.com/maui-toolkit/cartesian-charts/getting-started)
  - Used for revenue trends and quantity sold.
  - Supports `SplineAreaSeries` and `ColumnSeries`.
  - Provides gradient fills, tooltips, zooming, and touch support.

- [**SfCircularChart**](https://help.syncfusion.com/maui-toolkit/circular-charts/getting-started)
  - Used to display top-performing products.
  - Implemented as a doughnut chart with segment explosion and center content.
  - Includes smart data labels, gradient fills, and dynamic legends.

- [**SfRadialGauge**](https://help.syncfusion.com/maui/radial-gauge/getting-started)
  - Displays AI confidence levels in a visual meter format.
  - Color-coded pointers indicate low to high prediction reliability.
  - Animated needle and customizable appearance.

- [**SfMaps**](https://help.syncfusion.com/maui/maps/getting-started)
  - Visualizes regional sales distribution using custom markers.
  - Bubble marker sizes vary based on sales performance by region.
  - Supports shape data and intuitive map navigation.

- [**SfDataGrid**](https://help.syncfusion.com/maui/datagrid/getting-started)
  - Used for displaying orders and product inventory.
  - Features include sorting, filtering, paging, grouping, and virtualization.
  - Cells support images, ratings, and conditional formatting.

- [**SfAIAssistView**](https://help.syncfusion.com/maui/aiassistview/getting-started)
  - Provides an AI-powered conversational interface.
  - Users can query sales data using natural language.
  - Includes pre-suggested questions and chat context display.

## Troubleshooting
### Path too long exception
If you are facing a path too long exception when building this example project, close Visual Studio and rename the repository to a shorter name before building the project.

For a step-by-step procedure, refer to the [AI powered Sales Revenue Dashboard Blog]().

