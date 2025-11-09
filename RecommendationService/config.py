import os

DB_HOST = os.environ.get("DB_HOST", ".") 
DB_NAME = os.environ.get("DB_NAME", "ECommerce_Application") 

DRIVER_NAME = "ODBC+Driver+17+for+SQL+Server"


DB_CONNECTION_STRING = (
    f"mssql+pyodbc://{DB_HOST}/{DB_NAME}?"
    f"driver={DRIVER_NAME}"
    "&Trusted_Connection=yes"
    "&TrustServerCertificate=yes"
)


SOURCE_TABLE_NAME = "OrderItems"
RECOMMENDATION_TABLE_NAME = "ProductRecommendations"

TOP_N_RECOMMENDATIONS = 5