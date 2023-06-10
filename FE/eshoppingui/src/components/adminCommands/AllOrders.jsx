import { useEffect, useState } from "react"
import { useNavigate, Link } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import { CalculateCountdown, GetAllOrders } from "../../services/OrderServices"; 


export const AllOrders = () =>{
    const [allOrders, setAllOrders] = useState([]);
    const token = localStorage.getItem("token");
    const navigate = useNavigate();

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    } 

    const handleDetailedView = async(orderId) => {
        navigate("/order-details",{
            state:{
                pOrderId:orderId
            }
        })
    }
    
    useEffect( () => {
        const getOrders = async () => {
            try{
                const response = await GetAllOrders(handleAlert, token);
                setAllOrders(response);
            }
            catch(ex){
                console.log(ex);
            }
        };
        getOrders();
    }, []);

    useEffect(() => {
        const interval = setInterval(() => {
          setAllOrders((prevOrders) => {
            return prevOrders.map((order) => {
              const updatedOrder = { ...order };
              if (!updatedOrder.initialDeliveryTime) { 
                updatedOrder.initialDeliveryTime = updatedOrder.deliveringTime;
              }
    
              updatedOrder.deliveringTime = CalculateCountdown(updatedOrder.initialDeliveryTime);
    
              return updatedOrder;
            });
          });
        }, 1000);
        return () => clearInterval(interval);
      }, []);
    
      return(
        <>
        <Link className='link-button' to='/dashboard'>
            <button className="back-to-dashboard-button">Dashboard</button>
        </Link> 
        <ToastContainer/>
        {allOrders.length === 0 ?
            <p style={{color:"white"}}>You do not have any orders</p>
            :
            <table className="verify-sellers-table">
                <tr className="verify-sellers-table-header-row">
                    <th style={{display:"none"}}>Id</th>
                    <th>Number of products</th>
                    <th>Total price</th>
                    <th>Ordered At</th>
                    <th>Will be delivered in</th>
                    <th>Customer's comment</th>
                    <th>Details</th> 
                </tr>
                {allOrders.map(order => (
                    <tr key={order.id}>
                        <td style={{display:"none"}}>{order.id}</td>
                        <td>{order.numberOfProducts}</td>
                        <td>{order.totalPrice}</td>
                        <td>{order.orderedAt}</td>
                        <td>
                            {order.orderCanceled === true ?(
                                <p style={{color:"white"}}>Order is canceled</p>
                            ):
                            (order.deliveringTime)
                            }
                        </td>
                        <td>{order.comment}</td>
                        <td><button onClick={() => handleDetailedView(order.id)}>Details</button></td>
                    </tr>
                ))}
            </table>
        }
        </>
    )
}