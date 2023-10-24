namespace System.Web.Services;

[Flags]
public enum WsiProfiles {
    /* The web service claims to conform to the WSI Basic Profile version 1.1. */
    BasicProfile1_1	= 1,
    /* The web service makes no conformance claims. */
    None = 0,
}