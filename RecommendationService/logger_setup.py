import logging
import sys

def setup_logging():
    """Configures the root logger for the application."""
    
    logger = logging.getLogger() 
    logger.setLevel(logging.INFO) 

    if logger.hasHandlers():
        logger.handlers.clear()

    log_format = "[%(asctime)s] [%(levelname)s] [%(name)s] - %(message)s"
    formatter = logging.Formatter(log_format, datefmt="%Y-%m-%d %H:%M:%S")

    file_handler = logging.FileHandler("recommendation_job.log")
    file_handler.setFormatter(formatter)
    
    stream_handler = logging.StreamHandler(sys.stdout)
    stream_handler.setFormatter(formatter)

    logger.addHandler(file_handler)
    logger.addHandler(stream_handler)
    
    logging.info("Logging configured successfully.")