//Maya ASCII 2012 scene
//Name: maori_crowd_01.ma
//Last modified: Tue, Jun 11, 2013 12:28:14 AM
//Codeset: 1252
requires maya "2012";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2012";
fileInfo "version" "2012 x64";
fileInfo "cutIdentifier" "001200000000-796618";
fileInfo "osv" "Microsoft Windows 7 Home Premium Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
createNode transform -n "jap_crowd";
createNode mesh -n "jap_crowdShape" -p "jap_crowd";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 9.7656212005858833e-008 0.48967811465263367 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 4 ".uvst[0].uvsp[0:3]" -type "float2" 0.6521455 0.34195197
		 1 0.34195149 0.65214545 0.63740426 0.99999982 0.63740474;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr -s 4 ".pt[0:3]" -type "float3"  35.424171 -9.7383499 -33.141026 
		35.424171 -9.7383499 -18.046425 35.424171 -9.7383499 -33.141026 35.424171 -9.7383499 
		-18.046425;
	setAttr -s 4 ".vt[0:3]"  -35.42417145 9.73834991 33.14102554 -35.42417145 9.73834991 9.99495316
		 -35.42417145 16.576931 33.14102554 -35.42417145 16.576931 9.99495316;
	setAttr -s 4 ".ed[0:3]"  0 1 0 1 3 0 3 2 0 2 0 0;
	setAttr -s 4 ".n[0:3]" -type "float3"  1 0 0 1 0 0 1 0 0 1 0 0;
	setAttr ".fc[0]" -type "polyFaces" 
		f 4 0 1 2 3
		mu 0 4 0 1 3 2;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".vs" 7.987097;
	setAttr ".bw" 7.464516;
createNode materialInfo -n "materialInfo1";
createNode shadingEngine -n "pasted__pPlane1SG";
	setAttr ".ihi" 0;
	setAttr ".ro" yes;
createNode lambert -n "mat_crowd";
createNode file -n "pasted__file6";
	setAttr ".ftn" -type "string" "D:/Documents/Charles/Documents/PROJETS_UNITY/PFE/Assets/FBX/decors/Textures/DI_crowd.psd";
createNode place2dTexture -n "place2dTexture1";
createNode file -n "pasted__file7";
	setAttr ".ftn" -type "string" "D:/Documents/Charles/Documents/PROJETS_UNITY/PFE/Assets/FBX/decors/Textures/DI_crowd.psd";
createNode place2dTexture -n "place2dTexture2";
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 3 ".lnk";
	setAttr -s 3 ".slnk";
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :renderPartition;
	setAttr -s 3 ".st";
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultShaderList1;
	setAttr -s 3 ".s";
select -ne :defaultTextureList1;
	setAttr -s 2 ".tx";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
	setAttr -s 2 ".u";
select -ne :defaultRenderingList1;
select -ne :renderGlobalsList1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :defaultHardwareRenderGlobals;
	setAttr ".fn" -type "string" "im";
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
connectAttr "pasted__pPlane1SG.msg" "materialInfo1.sg";
connectAttr "mat_crowd.msg" "materialInfo1.m";
connectAttr "pasted__file6.msg" "materialInfo1.t" -na;
connectAttr "mat_crowd.oc" "pasted__pPlane1SG.ss";
connectAttr "jap_crowdShape.iog" "pasted__pPlane1SG.dsm" -na;
connectAttr "pasted__file6.oc" "mat_crowd.c";
connectAttr "pasted__file7.ot" "mat_crowd.it";
connectAttr "place2dTexture1.o" "pasted__file6.uv";
connectAttr "place2dTexture1.ofu" "pasted__file6.ofu";
connectAttr "place2dTexture1.ofv" "pasted__file6.ofv";
connectAttr "place2dTexture1.rf" "pasted__file6.rf";
connectAttr "place2dTexture1.reu" "pasted__file6.reu";
connectAttr "place2dTexture1.rev" "pasted__file6.rev";
connectAttr "place2dTexture1.vt1" "pasted__file6.vt1";
connectAttr "place2dTexture1.vt2" "pasted__file6.vt2";
connectAttr "place2dTexture1.vt3" "pasted__file6.vt3";
connectAttr "place2dTexture1.vc1" "pasted__file6.vc1";
connectAttr "place2dTexture1.ofs" "pasted__file6.fs";
connectAttr "place2dTexture2.o" "pasted__file7.uv";
connectAttr "place2dTexture2.ofu" "pasted__file7.ofu";
connectAttr "place2dTexture2.ofv" "pasted__file7.ofv";
connectAttr "place2dTexture2.rf" "pasted__file7.rf";
connectAttr "place2dTexture2.reu" "pasted__file7.reu";
connectAttr "place2dTexture2.rev" "pasted__file7.rev";
connectAttr "place2dTexture2.vt1" "pasted__file7.vt1";
connectAttr "place2dTexture2.vt2" "pasted__file7.vt2";
connectAttr "place2dTexture2.vt3" "pasted__file7.vt3";
connectAttr "place2dTexture2.vc1" "pasted__file7.vc1";
connectAttr "place2dTexture2.ofs" "pasted__file7.fs";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "pasted__pPlane1SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "pasted__pPlane1SG.message" ":defaultLightSet.message";
connectAttr "pasted__pPlane1SG.pa" ":renderPartition.st" -na;
connectAttr "mat_crowd.msg" ":defaultShaderList1.s" -na;
connectAttr "pasted__file6.msg" ":defaultTextureList1.tx" -na;
connectAttr "pasted__file7.msg" ":defaultTextureList1.tx" -na;
connectAttr "place2dTexture1.msg" ":defaultRenderUtilityList1.u" -na;
connectAttr "place2dTexture2.msg" ":defaultRenderUtilityList1.u" -na;
// End of maori_crowd_01.ma
