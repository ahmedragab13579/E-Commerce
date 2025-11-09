import pandas as pd
from sqlalchemy import create_engine
import logging
import config

logger = logging.getLogger(__name__)

def load_order_items(connection_string: str) -> pd.DataFrame:
    """
    Loads all OrderId/ProductId pairs from the database
    by calling the 'sp_GetOrderItems' stored procedure.
    """
    logger.info("Connecting to database to execute 'sp_GetOrderItems'...")
    
    try:
        engine = create_engine(connection_string)
       
        sql_query = "EXEC sp_GetOrderItems"
        
       

        with engine.connect() as conn:
            order_items_df = pd.read_sql(sql_query, conn)
            
        logger.info(f"Successfully loaded {len(order_items_df)} order items from SP.")
        return order_items_df
    
    except Exception as e:
        logger.error(f"Failed to execute stored procedure 'sp_GetOrderItems': {e}")
        raise