import pandas as pd
from sqlalchemy import create_engine, text
import logging
import config

logger = logging.getLogger(__name__)

def write_recommendations(recommendations_df: pd.DataFrame, connection_string: str):
    
    table_name = config.RECOMMENDATION_TABLE_NAME
    
    if recommendations_df.empty:
        logger.warning("Recommendation DataFrame is empty. Nothing to write.")
        return

    try:
        engine = create_engine(connection_string)
        
        with engine.begin() as conn: 
            
            logger.info(f"Starting transaction to replace data in '{table_name}'...")
            
            logger.info(f"Calling sp_ClearRecommendations to truncate old data...")
            conn.execute(text("EXEC sp_ClearRecommendations"))
            
            logger.info(f"Writing {len(recommendations_df)} new records using pandas.to_sql...")
            
            recommendations_df.to_sql(
                table_name, 
                conn,  
                if_exists='append', 
                index=False,
                chunksize=1000
            )
            
            logger.info("Transaction committed. Data successfully replaced.")
        
    except Exception as e:
        logger.error(f"Failed to write recommendations (transaction rolled back): {e}")
        raise