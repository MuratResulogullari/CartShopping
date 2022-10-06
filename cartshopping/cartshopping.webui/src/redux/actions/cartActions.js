import * as actionTypes from "./actionTypes";
import apiFetch from "./apiFetch";

/** CRUDs  */

export function addToCartSuccess(cartItem) {
    return { type: actionTypes.ADD_TO_CART_SUCCESS, payload: cartItem }
}
export function deleteFromCartSuccess(cartItem) {
    return { type: actionTypes.DELETE_FROM_CART_SUCCESS, payload: cartItem }
}
export const getCartItemsSuccess = (cartItems) => {
    return { type: actionTypes.GET_CARTITEMS_SUCCESS, payload: cartItems };
}
export const getCountSuccess = (count) => {
    return { type: actionTypes.GET_COUNT_SUCCESS, payload: count };
}
export const getCartItems = () => {
    return async (dispatch) => {
        var url = 'https://localhost:7086/api/Cart/GetCart';
        await apiFetch(url, 'GET').then(data => {
            if (data.IsSuccess) {
                return dispatch(getCartItemsSuccess(data.Result.CartItems))
            }
            else {
                console.error(data.Message);
            }
        }).catch(handleError)
    }
}
export const addToCart = (productId, quantity) => {
    debugger;
    return async (dispatch) => {
        var url = 'https://localhost:7086/api/Cart/AddToCart/' + productId + '/' + quantity;
        await apiFetch(url, 'POST')
            .then(data => {
                if (data.isSuccess) {
                    return dispatch(addToCart(data.result))
                }
                else {
                    var error = data.message;
                    console.log("addToCart ", error);
                }
            }).catch(handleError);
    }
}
export const deleteFromCart = (cartItemId) => {
    return async (dispatch) => {
        var url = 'https://localhost:7086/api/Cart/DeleteFromCart/' + cartItemId;
        await apiFetch(url, 'POST')
            .then(data => {
                if (data.isSuccess) {
                    return dispatch(deleteFromCartSuccess(data.result))
                }
                else {
                    var error = data.message;
                    console.log("deleteFromCart ", error);
                }
            }).catch(handleError);
    }
}

/** Errors  */
export async function handleResponse(response) {
    if (response.ok) {
        return response.json();
    }
    else {
        const error = await response.text()
        throw new Error(error);
    }
}
export function handleError(error) {
    console.error("Resulted error : " + error);
    throw error;
}