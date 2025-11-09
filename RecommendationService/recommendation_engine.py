import pandas as pd
import logging
from itertools import permutations 
logger = logging.getLogger(__name__)

def generate_recommendations(order_items_df: pd.DataFrame, top_n: int) -> pd.DataFrame:
    """
    Generates product recommendations based on co-purchase frequency.
    This version is optimized for memory and performance.
    """
    if order_items_df.empty:
        logger.warning("Input DataFrame is empty. Returning empty recommendations.")
        return pd.DataFrame(columns=['ProductId', 'RecommendedProductId', 'Score'])

    try:
        logger.info("Starting recommendation analysis (Efficient Method)...")
        
        logger.debug("Grouping products by OrderId...")
        order_groups = order_items_df.groupby('OrderId')['ProductId'].apply(list)

      
        logger.debug("Generating co-purchase pairs using permutations...")
        all_pairs = order_groups.apply(lambda x: list(permutations(x, 2)))
        
        all_pairs_exploded = all_pairs.explode()

        all_pairs_exploded = all_pairs_exploded.dropna()

        if all_pairs_exploded.empty:
            logger.warning("No co-purchases found. No recommendations to generate.")
            return pd.DataFrame(columns=['ProductId', 'RecommendedProductId', 'Score'])

        logger.debug("Counting pair frequencies...")
        recommendations = all_pairs_exploded.value_counts().reset_index(name='Score')
        recommendations.columns = ['Pair', 'Score']

        recommendations[['ProductId_A', 'ProductId_B']] = pd.DataFrame(recommendations['Pair'].tolist(), index=recommendations.index)

        logger.debug("Sorting, ranking, and finalizing recommendations...")
        recommendations = recommendations.sort_values(by=['ProductId_A', 'Score'], ascending=[True, False])
        final_recommendations = recommendations.groupby('ProductId_A').head(top_n)
        
        final_recommendations = final_recommendations[['ProductId_A', 'ProductId_B', 'Score']].rename(columns={
            'ProductId_A': 'ProductId',
            'ProductId_B': 'RecommendedProductId'
        })
        
        logger.info(f"Generated {len(final_recommendations)} recommendation pairs.")
        return final_recommendations

    except Exception as e:
        logger.error(f"Failed during recommendation generation: {e}")
        raise