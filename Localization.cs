using System.Collections.Generic;

namespace Canals_Blender
{
    public static class Localization
    {
        public static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            {
                "Français", new Dictionary<string, string>
                {
                    { "WindowTitle", "Canals Blender" },
                    { "BtnDownload", "Télécharger" },
                    { "BtnDelete", "Supprimer les images" },
                    { "MsgErrorTitle", "Erreur" },
                    { "MsgNoImage", "Veuillez glisser et déposer une image à télécharger." },
                    { "MsgSaveError", "Échec de l’enregistrement" },
                    { "MsgInvalidFile", "Le fichier doit être au format JPEG, JPG ou PNG." },

                    { "L_French", "Français" },
                    { "L_English", "Anglais" }
                }
            },
            {
                "English", new Dictionary<string, string>
                {
                    { "WindowTitle", "Canals Blender" },
                    { "BtnDownload", "Download" },
                    { "BtnDelete", "Delete images" },
                    { "MsgErrorTitle", "Error" },
                    { "MsgNoImage", "Please drag and drop any image to download." },
                    { "MsgSaveError", "Save failed" },
                    { "MsgInvalidFile", "The file must be a JPEG, JPG or PNG." },

                    { "L_French", "French" },
                    { "L_English", "English" }
                }
            }
        };
    }
}