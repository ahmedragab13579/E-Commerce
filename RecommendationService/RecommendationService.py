import logging
import config 
import sys    
import argparse 
from logger_setup import setup_logging
from data_loader import load_order_items
from recommendation_engine import generate_recommendations
from data_writer import write_recommendations

setup_logging()
logger = logging.getLogger(__name__)

def run_job(connection_string: str, top_n: int):
    """
    Main function to run the entire recommendation pipeline.
    """
    logger.info("==============================================")
    logger.info(f"STARTING RECOMMENDATION JOB (Top-N={top_n})")
    logger.info("==============================================")
    
    try:
        order_items_df = load_order_items(connection_string)
        
        if order_items_df.empty:
            logger.warning("No order items found. Job finished.")
            return

        recommendations_df = generate_recommendations(
            order_items_df, 
            top_n=top_n
        )

        if recommendations_df.empty:
            logger.warning("No recommendations were generated. Job finished.")
            return

        write_recommendations(
            recommendations_df, 
            connection_string
        )
        
        logger.info("==============================================")
        logger.info("RECOMMENDATION JOB COMPLETED SUCCESSFULLY")
        logger.info("==============================================")

    except Exception as e:
        logger.error(f"JOB FAILED: An unhandled exception occurred: {e}", exc_info=True)
        logger.info("==============================================")
        logger.info("RECOMMENDATION JOB FAILED")
        logger.info("==============================================")
        
        sys.exit(1) 

def main():
    """
    Parses command-line arguments and runs the job.
    """
    parser = argparse.ArgumentParser(description="Run the recommendation generation job.")
    
    parser.add_argument(
        "--conn_str",
        type=str,
        default=config.DB_CONNECTION_STRING, 
        help="Database connection string."
    )
    
    parser.add_argument(
        "--top_n",
        type=int,
        default=config.TOP_N_RECOMMENDATIONS, 
        help="Number of recommendations to generate per product."
    )
    
    args = parser.parse_args()

    run_job(connection_string=args.conn_str, top_n=args.top_n)


if __name__ == "__main__":
    main()