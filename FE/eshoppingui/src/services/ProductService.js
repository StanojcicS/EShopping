import axios from "axios";

export const GetSellersProducts = async (handleAllert, token) =>
{
    try{
        const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/seller/products`,
        {
            headers: {
                Authorization : `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch(ex)
    {
        console.error("Error while trying to get list of your products: ", ex.response.data.message);
        handleAllert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const GetAllProducts = async (handleAlert, token) =>
{
    try{
        const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/products`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch(ex)
    {
        console.error("Error while trying to get list of products: ", ex.response.data.message);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const DeleteProduct = async (handleAllert, token, productId) =>
{
    try{
        const response = await axios.delete(`${process.env.REACT_APP_API_BASE_URL}/products/${productId}/delete`,
        {
            headers: {
                Authorization : `Bearer ${token}`
            },
            data: {
                undefined,
                productId
            }
        });
    }
    catch(ex)
    {
        console.error("Error while trying to delete this product: ", ex.response.data.message);
        handleAllert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const AddProduct = async (name, price, quantity, description, image, handleAllert, navigate, token) =>
{
    try{

        if(name === "" || price === "" || quantity === "" || description === "" || !image)
        {
            handleAllert("All fields are mandatory.", "error");
            return;
        }

        const formData = new FormData();
        formData.append("name", name);
        formData.append("price", price);
        formData.append("quantity", quantity);
        formData.append("description", description);
        formData.append("image", image);

        const response = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/product`,
            formData,
            {
            headers: {
                Authorization : `Bearer ${token}`
            }
            });

        handleAllert("Successfully added new product.", "success");
        setTimeout(() => {navigate("/my-products");}, 2000);
        return response;
    }
    catch(ex)
    {
        console.error("Error while trying to add new product: ", ex.response);
        handleAllert(ex.response.data.message, "error");
        return ex.response;
    }
}

export const UpdateProduct = async (handleAlert, 
                                    navigate, 
                                    token, 
                                    id, 
                                    name, 
                                    price, 
                                    quantity, 
                                    description,  
                                    updatedImage) =>
{
    try{

        if(name === "" || price === "" || quantity === "" || description === "")
        {
            handleAlert("All fields are mandatory.", "error");
            return;
        }

        const formData = new FormData();
        formData.append("sellerId", null);
        formData.append("productId", id);
        formData.append("name", name);
        formData.append("price", price);
        formData.append("quantity", quantity);
        formData.append("description", description);
        if(updatedImage)
        {
            formData.append("updatedImage", updatedImage);
        }

        const response = await axios.put(`${process.env.REACT_APP_API_BASE_URL}/products/${id}/update`,
            formData,
            {
            headers: {
                Authorization : `Bearer ${token}`
            }
            });

        handleAlert("Successfully updated product.", "success");
        setTimeout(() => {navigate("/my-products");}, 2000);
        return response;
    }
    catch(ex)
    {
        console.error("Error while trying to add new product: ", ex.response);
        handleAlert(ex.response.data.message, "error");
        return ex.response;
    }
}