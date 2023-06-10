import { useEffect, useState } from "react"
import {ToastContainer, toast} from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css'; 
import { GetAllProducts } from "../../services/ProductService";
import { PlaceNewOrder } from "../../services/OrderServices"; 
import { useNavigate, Link } from "react-router-dom";

export const NewOrder = () => {
    const [allProducts, setAllProducts] = useState([]);
    const [productIds, setProductIds] = useState([]);
    const token = localStorage.getItem("token");
    const [deliveryAddress, setDeliveryAddress] = useState('');
    const [comment, setComment] = useState('');
    const navigate = useNavigate();

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    const addToCart = (productId, quantity) => {  
        for(let i = 0 ; i < quantity; i++)
        {
            setProductIds((prevItems) => [...prevItems, productId]);                  
        } 
    }

    const placeOrder = async (e) => {
        console.log(productIds);
        e.preventDefault();
        const response = await PlaceNewOrder(productIds, comment, deliveryAddress, token, handleAlert);
        if(response.data.message === "There is no more this product")
        {
            handleAlert("This product was removed from the shop or quantity in warehouse is decreased, please refresh and try again later.");
        }
        if (response !== undefined && response !== "")
        {
            setProductIds([]);
        }
    }

    useEffect( () => {
        const getAllProducts = async () => {
            try{
                const response = await GetAllProducts(handleAlert, token);
                setAllProducts(response);
            }
            catch(ex){
                console.log(ex);
            }
        };
        getAllProducts();
    }, []);
    return(
        <>
        <ToastContainer/>
        <Link className='link-button' to='/dashboard'>
            <button className="back-to-dashboard-button">Dashboard</button>
        </Link> 
        <h2 style={{color:"white"}}>All products</h2>
        {allProducts.length === 0 ?
            <p style={{color:"white"}}>Sorry, there is no available products at the moment, please come back later.</p>    
            :
            <table className='verify-sellers-table'>
                <tr className="verify-sellers-table-header-row">
                    <th style={{display:"none"}}>Id</th>
                    <th>Image</th>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Available quantity</th>
                    <th>Description</th>
                    <th>Quantity</th>
                </tr>
                {allProducts.map(product => (
                    <>
                    <tr key={product.id}>
                        <td style={{display:"none"}}>{product.id}</td>
                        <td>
                            <img className="profile-picture" src={product.image} alt="No picture"/>
                        </td>
                        <td>{product.name}</td>
                        <td>{product.price}</td>
                        <td>{product.quantity}</td>
                        <td>{product.description}</td>
                        <td>
                            <input style={{width:"30px", textAlign:"center"}}   type="number" min="1" max={product.quantity}
                            defaultValue="1" id={`quantity_${product.id}`}
                            />
                            <button style={{marginLeft:"5px"}}
                            onClick={() => {
                                const quantityInput = document.getElementById(`quantity_${product.id}`);
                                const quantity = parseInt(quantityInput.value); 
                                if(quantity <= product.quantity && quantity > 0){
                                    addToCart(product.id, quantity);
                                     quantityInput.value = "1";
                                    handleAlert("Successfully added product(s) to the cart.", "success");
                                }
                                else {
                                    handleAlert(`You can order between 0 and ${product.quantity} items called '${product.name}'`, "error");
                                }
                            }}>
                            Add to cart        
                            </button>
                        </td>
                    </tr>
                    </>
                ))}
            </table>    
        }
        <form className="place-new-order-form" onSubmit={placeOrder}>
                            <label htmlFor="deliveryAddress">Delivery address:</label>
                            <input value={deliveryAddress} onChange={(e) => setDeliveryAddress(e.target.value)} type="deliveryAddress" placeholder="Enter your address" id="deliveryAddress" name="deliveryAddress"/>
                            <label htmlFor="comment">Comment:</label>
                            <input value={comment} onChange={(e) => setComment(e.target.value)} type="comment" placeholder="Anything we need to know?" id="comment" name="comment"/>
                            <button type="submit" >Place order</button>
                    </form>
        </>
    )
}