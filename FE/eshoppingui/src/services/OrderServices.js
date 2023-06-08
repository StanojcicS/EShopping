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
    
        const deliveringDateTime = new Date(response.data.deliveringDateTime);
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