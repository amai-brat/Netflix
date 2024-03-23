import infoStyles from '../styles/subscriptionInfo.module.scss';
import checkMark from '../images/check-mark.svg';

export const SubscriptionInfo = ({ subscription, showPurchase, setSubscriptionId, setModalIsOpen }) => {
    function handlePurchaseClick(event) {
        setSubscriptionId(event.target.dataset.id);
        setModalIsOpen(true);
    }
    
    return (
      <div className={infoStyles.subscriptionWrapper}>
          <div className={infoStyles.header}>
              <p className={infoStyles.name}>{subscription.name}</p>
              <div className={infoStyles.line}></div>
              <p className={infoStyles.price}>{subscription.price}</p>
          </div>
          <div className={infoStyles.infos}>
              {subscription.infos.map((info, index) => (
                  <div key={index} className={infoStyles.info}>
                      <p className={infoStyles.infoText}>{info}</p>
                      <img className={infoStyles.checkMark} width={20} height={20} src={checkMark} alt={"✓"} />
                  </div>
              ))}
          </div>
          {showPurchase && 
              (subscription.isCurrentPurchased
                ? (<div className={infoStyles.purchased + " " + infoStyles.footer}>Ваша текущая подписка</div>)
                : (<div data-id={subscription.id} 
                        className={infoStyles.notPurchased + " " + infoStyles.footer} 
                        onClick={handlePurchaseClick}>Купить</div>)
              )}
      </div>  
    );
};

