import { useEffect, useState } from "react";
import axios from "axios";
import Cookies from "js-cookie";

export default function CreateUser() {
    const [username, setUsername] = useState("")
    const [password, setPassword] = useState("")
    const [msg, setMsg] = useState("");
    const [role, setRole] = useState(1)
    const API_URL = import.meta.env.VITE_APP_API_URL;

    useEffect(() => {
        if (Cookies.get('role') != 0) {
            window.location.href = '/';
        }
    }, [])

    const CreateUser = async (e) => {
        e.preventDefault();
        let data = JSON.stringify({
            "username": username,
            "password": password,
            "role": role
        });

        let config = {
            method: 'post',
            maxBodyLength: Infinity,
            url: API_URL + '/CreateUser/' + Cookies.get("id"),
            headers: {
                'Content-Type': 'application/json'
            },
            data: data
        };

        axios.request(config)
            .then((response) => {
                setMsg(response.data.message);
            })
            .catch((error) => {
                console.log(error);
            });
    }


    return (
        <div>
            <div className="space-y-4">
                <button
                    onClick={() => { window.location.href = "/" }}
                    className="w-40 bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition"
                >
                    voltar à tela principal
                </button>
                <h2>Criar Usuário</h2>
                <form onSubmit={CreateUser} className="space-y-4">
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
                    <div>
                        <label className="block text-gray-700">Função</label>
                        <select
                            className="w-full mt-1 p-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={role}
                            onChange={(e) => setRole(e.target.value)}
                            required
                        >
                            <option value="0">Admin</option>
                            <option value="1">Basic</option>
                        </select>
                    </div>
                    <button
                        type="submit"
                        className="w-full bg-green-500 text-white py-3 rounded-lg hover:bg-green-600 transition"
                    >
                        Cadastrar
                    </button>
                </form>
                <h2 className="bg-background-green-500 ">
                    {msg != "" ?
                        <>{msg}</> :
                        ""}
                </h2>
            </div>
        </div>
    );
}