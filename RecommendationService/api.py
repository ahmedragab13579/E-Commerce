import uvicorn
from fastapi import FastAPI, HTTPException
from sqlalchemy import create_engine, text
import pandas as pd
import config  
import logging

app = FastAPI(
    title="Recommendation API",
    description="API to retrieve pre-calculated product recommendations."
)

logger = logging.getLogger("uvicorn")

try:
    engine = create_engine(config.DB_CONNECTION_STRING)
    logger.info("Database engine created successfully.")
except Exception as e:
    logger.error(f"FATAL: Could not create database engine: {e}")
    engine = None


@app.get("/recommendations/{product_id}")
def get_recommendations_for_product(product_id: int):
    """
    Fetches pre-calculated recommendations for a given Product ID.
    """
    if engine is None:
        raise HTTPException(status_code=500, detail="Database connection is not configured.")

    table_name = config.RECOMMENDATION_TABLE_NAME
    
    sql_query = text(f"""
        SELECT RecommendedProductId, Score 
        FROM {table_name} 
        WHERE ProductId = :p_id 
        ORDER BY Score DESC
    """)
    
    try:
        logger.info(f"Fetching recommendations for ProductId: {product_id}")
        with engine.connect() as conn:
            result_df = pd.read_sql(
                sql_query, 
                conn, 
                params={"p_id": product_id}
            )

        if result_df.empty:
            logger.warning(f"No recommendations found for ProductId: {product_id}")
            raise HTTPException(status_code=404, detail="No recommendations found for this product.")

        recommendations = result_df.to_dict('records')
        
        return {
            "product_id": product_id,
            "recommendations": recommendations
        }

    except HTTPException as e:
        raise e
    except Exception as e:
        logger.error(f"Error fetching recommendations for {product_id}: {e}", exc_info=True)
        raise HTTPException(status_code=500, detail=f"An internal error occurred: {str(e)}")


if __name__ == "__main__":
    logger.info("Starting Uvicorn server in development mode...")
    
    uvicorn.run(
        "api:app",  
        host="127.0.0.1", 
        port=8000, 
        reload=True 
    )
