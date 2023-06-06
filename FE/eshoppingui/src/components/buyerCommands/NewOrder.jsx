import { useEffect, useState } from "react"
import {ToastContainer, toast} from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css'; 
import { GetAllProducts } from "../../services/ProductService";

export const NewOrder = () => {
    const [allProducts, setAllProducts] = useState([]);
    const token = localStorage.getItem("token");

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    const addToCart = () => {

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
                    <th></th>
                </tr>
                {allProducts.map(product => (
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
                            <input type="number" min="1" max={product.quantity}
                            defaultValue="1" id={`quantity_${product.id}`}
                            />
                            <button 
                            onClick={() => {
                                const quantity = parseInt(document.getElementById(`quantity_${product.id}`).value);
                                if(quantity <= product.quantity){
                                    addToCart(product.Id, quantity);
                                    quantity = "1";
                                    handleAlert("Successfully added product(s) to the cart.", "success");
                                }
                                else {
                                    handleAlert(`Only ${product.quantity} product(s) available for ${product.name}`, "error");
                                }
                            }}>

                            </button>
                        </td>
                    </tr>
                ))}
            </table>    
        }
        </>
    )
}