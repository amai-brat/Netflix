namespace SupportAPI.Models.Abstractions.FileTypes;

public interface IHavingAudioFileType: IHavingFiletype
{
    FileType IHavingFiletype.Type
    {
        get => FileType.Audio;
        set => Type = value;
    }
}