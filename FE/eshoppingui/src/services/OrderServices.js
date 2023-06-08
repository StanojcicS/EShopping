import axios from "axios";

export const PlaceNewOrder = async(productIds, comment, deliveryAddress, token, handleAlert, navigate) =>
{
    try{
        if(productIds.length === 0){
            handleAlert("Please add some products to your cart and then place an order.", "error");
            return;
        }
        if(deliveryAddress === ""){
            handleAlert("You have to enter delivery address", "error");
            return;
        }
        const response = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/order`,
        {
            comment,
            deliveryAddress,
            productIds
        },
        {
            headers: {
                Authorization : `Bearer ${token}`
            }
        }
    );
    
        const deliveringDateTime = new Date(response.data.deliveringTime);
        const displayedDateTme = `${deliveringDateTime.getHours()}:${deliveringDateTime.getMinutes()} - ${deliveringDateTime.getDate()}.${deliveringDateTime.getMonth() + 1}.${deliveringDateTime.getFullYear()}`;
        handleAlert(`Successfully placed an order. It will arive at ${displayedDateTme}`, "success");
        return response;
    }
    catch(ex)
    {
        console.error("Error while trying to place a new order: ", ex.response);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const GetBuyersOrders = async (handleAlert, token) =>
{
    try{
        const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/buyer/orders`,
        {
            headers: {
                Authorization : `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch(ex)
    {
        console.error("Error while trying to get buyers orders: ", ex.response.data.message);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const CalculateCountdown = (deliveringTime) => {
    const now = new Date().getTime();
    const deliveryTime = new Date(deliveringTime).getTime();
    const countdown = deliveryTime - now; 
    
    if(countdown < 0){
        return "Delivered";
    }
    
    const days = Math.floor(countdown / (1000 * 60 * 60 * 24));
    const hours = Math.floor((countdown % (1000 * 60 * 60 *24)) / (1000 * 60 * 60));
    const minutes = Math.floor((countdown % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((countdown % (1000 * 60)) / 1000);

    return `${days}:${hours}:${minutes}:${seconds}`;
}

export const CanOrderBeCanceled = (order) => {
    const cancelationTime = new Date(order.orderedAt);
    cancelationTime.setHours(cancelationTime.getHours() + 1);
    return cancelationTime > new Date();
}

export const CancelOrder = async (token, orderId, handleAlert, navigate) => {
    try{
        const response = await axios.put(`${process.env.REACT_APP_API_BASE_URL}/order/cancel`,
        {
            undefined,
            orderId
        },
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        handleAlert("Successfully canceled your order.", "success");
        setTimeout(() => {navigate("/old-orders");}, 2000);
        return response;
    }
    catch(ex)
    {
        console.error("Error while trying to cancel your order: ", ex.response.data.message);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}