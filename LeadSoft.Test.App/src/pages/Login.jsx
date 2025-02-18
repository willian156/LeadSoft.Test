import { useEffect, useState } from "react";
import axios from "axios";
import Cookies from "js-cookie";

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [msg, setMsg] = useState("");
    const API_URL = import.meta.env.VITE_APP_API_URL;
    
    useEffect(() => {
        alocateToPages()
    }, [])


    const alocateToPages = () => {

        if(Cookies.get('role') == 0){
            window.location.href = '/UserList';
        }else if (Cookies.get('role') ==1){
            window.location.href = '/ProcessedItens';
        }
    }

    const CreateStarterLogin = () => {
        debugger;
        let data = JSON.stringify({
            "username": username,
            "password": password,
            "role": 0
          });
          
          let config = {
            method: 'post',
            maxBodyLength: Infinity,
            url: API_URL+'/CreateStarterUser',
            headers: { 
              'Content-Type': 'application/json'
            },
            data : data
          };
          
          axios.request(config)
          .then((response) => {
            setMsg(response.data.message);
          })
          .catch((error) => {
            console.log(error);
          });
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        let config = {
            method: 'post',
            maxBodyLength: Infinity,
            url: API_URL + "/Login?username=" + username + "&password=" + password,
            headers: {}
        };

        axios.request(config)
            .then((response) => {

                Cookies.set('id', response.data.id, { path: "/", expires: 1 });
                Cookies.set('username', response.data.username, { path: "/", expires: 1 });
                Cookies.set('role', response.data.role, { path: "/", expires: 1 });

                alocateToPages();
            })
            .catch((error) => {
                console.log(error);
            });
    };

    return (
        <div>
            <div className="space-y-4">
                <h2>
                    Login
                </h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-gray-700">Username</label>
                        <input
                            type="text"
                            className="w-full mt-1 p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-700">Senha</label>
                        <input
                            type="password"
                            className="w-full mt-1 p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button
                        type="submit"
                        className="w-full bg-green-500 text-white py-3 rounded-lg hover:bg-green-600 transition"
                    >
                        Entrar
                    </button>
                </form>
                    <button
                        onClick={() => CreateStarterLogin()}
                        className="w-full bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition"
                    >
                        Cadastrar Admin
                    </button>
                <h2 className="bg-background-green-500 ">
                    {msg != "" ?
                        <>{msg}</> :
                        ""}
                </h2>
            </div>
        </div>
    );
}
