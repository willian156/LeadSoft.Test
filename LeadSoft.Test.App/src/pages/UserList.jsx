import { useEffect, useState } from "react";
import axios from "axios";
import Cookies from "js-cookie";


export default function UserList() {
    const [users, setUsers] = useState([]);
    const API_URL = import.meta.env.VITE_APP_API_URL;

    useEffect(() => {
        if (Cookies.get('role') != 0) {
            window.location.href = '/';
        }
    }, [])

    useEffect(() => {
        GetUsers()
    }, [])

    const GetUsers = () => {

        let AdminId = Cookies.get('id');

        let config = {
            method: 'get',
            maxBodyLength: Infinity,
            url: API_URL + '/GetUsers/' + AdminId,
            headers: {}
        };

        axios.request(config)
            .then((response) => {
                let test = response.data;
                setUsers(test);
            })
            .catch((error) => {
                console.log(error);
            });

    }


    return (
        <div className="p-4">
            <div className="flex space-x-4 mb-4">
                <button
                    onClick={() => { window.location.href = "/CreateUser"; }}
                    className="w-40 bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition"
                >
                    Cadastrar Usuário
                </button>
                <button
                    onClick={() => { window.location.href = "/StartProcess"; }}
                    className="w-40 bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 transition"
                >
                    Iniciar Processamento
                </button>
            </div>
            <h1 className="text-2xl font-bold mb-4">Usuários cadastrados:</h1>
            <table className="min-w-full bg-white border border-gray-200">
                <thead>
                    <tr className="bg-gray-100">
                        <th className="py-2 px-4 border-b">ID</th>
                        <th className="py-2 px-4 border-b">Username</th>
                        <th className="py-2 px-4 border-b">Password</th>
                        <th className="py-2 px-4 border-b">Role</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map((item) => (
                        <tr key={item.id} className="text-center border-b">
                            <td className="py-2 px-4">{item.id}</td>
                            <td className="py-2 px-4">{item.username}</td>
                            <td className="py-2 px-4">{item.password}</td>
                            <td className="py-2 px-4">
                                {item.role === 0 ? "Admin" : "Basic"}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );

}