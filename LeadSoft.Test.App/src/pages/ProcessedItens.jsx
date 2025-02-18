import { useEffect, useState } from "react";
import Cookies from "js-cookie";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

export default function ProcessedItens() {
    const [conn, setConn] = useState(null);
    const [processedLines, setProcessedLines] = useState(0);
    const [processedItems, setProcessedItems] = useState([]);
    const [processStatus, setProcessStatus] = useState(0);
    const WS_URL = import.meta.env.VITE_APP_WSS_URL;

    useEffect(() => {
        if (Cookies.get('role') != 1) {
            window.location.href = '/';
        }
    }, [])

    useEffect(() => {
        const joinListener = async () => {
            try {

                const connection = new HubConnectionBuilder()
                    .withUrl(WS_URL)
                    .configureLogging(LogLevel.Information)
                    .build();


                connection.on("ProcessingUpdate", (update) => {

                    if (update && update.processedLines !== undefined && update.processedItem) {

                        setProcessedLines(update.processedLines);
                        setProcessedItems((prevItems) => [...prevItems, update.processedItem]);
                    }
                });

                connection.on("ProcessingCompleted", (completed) => {
                    if (completed) {
                        setProcessStatus(1);
                    }
                })

                connection.on("ProcessingCanceled", (canceled) => {
                    if (canceled) {
                        setProcessStatus(2);
                    }
                })
                
                connection.on("ProcessingStarted",(started) => {
                    setProcessStatus(0);
                } )

                await connection.start();
                await connection.invoke(
                    "ProcessData",
                    `${Cookies.get("id")}_${Cookies.get("username")}`
                );

                setConn(connection);
            } catch (e) {
                console.error("Erro ao conectar ao SignalR:", e);
            }
        };

        joinListener();

        return () => {
            if (conn) {
                conn.stop();
            }
        };
    }, []);

    return (
        <div className="p-4">
            {processStatus == 1 ?
                <h1 className="bg-green-500 text-2xl font-bold mb-4 bg-color">Processamento Completado!</h1>
                : processStatus == 2 ?
                    <h1 className="bg-red-500 text-2xl font-bold mb-4 bg-color">Processamento Cancelado!</h1>
                    : <>
                    </>}
            <h1 className="text-2xl font-bold mb-4">Itens Processados</h1>
            <p className="mb-4">Total de linhas processadas: {processedLines}</p>
            <div className="overflow-y-auto max-h-150 border rounded-lg shadow-md">
                <table className="min-w-full bg-white border">
                    <thead className="bg-gray-100 sticky top-0">
                        <tr>
                            <th className="py-2 px-4 border-b">ID</th>
                            <th className="py-2 px-4 border-b">Título</th>
                            <th className="py-2 px-4 border-b">Gênero</th>
                            <th className="py-2 px-4 border-b">Tipo</th>
                            <th className="py-2 px-4 border-b">Fonte</th>
                        </tr>
                    </thead>
                    <tbody>
                        {processedItems.map((item) => (
                            <tr key={item.id} className="border-b text-center">
                                <td className="py-2 px-4">{item.id}</td>
                                <td className="py-2 px-4">{item.title}</td>
                                <td className="py-2 px-4">{item.gender}</td>
                                <td className="py-2 px-4">{item.type}</td>
                                <td className="py-2 px-4">{item.source}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}